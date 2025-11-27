using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using static System.Net.WebRequestMethods;

namespace PriceFeedAPI
{
    public enum CompressionStatus
    {
        ON,
        OFF
    }

    public class MarketData
    {
        public uint MktSegId { get; set; }
        public uint Token { get; set; }
        public uint LUT { get; set; }
        public uint LTP { get; set; }
        public uint ClosePrice { get; set; }
        public uint DecimalLocator { get; set; }
    }

    public class ODINMarketFeedClient : IDisposable
    {
        private ClientWebSocket _webSocket;
        private CompressionStatus _compressionStatus = CompressionStatus.ON;
        private string _userId = "";
        private CancellationTokenSource _cts;
        private bool _isDisposed = false;
        private int ReceiveBufferSize = 8192;

        public event Action OnOpen;
        public event Action<object> OnMessage;
        public event Action<string> OnError;
        public event Action<int, string> OnClose;
        private FragmentationHandler fragHandler = new FragmentationHandler();

        public ODINMarketFeedClient()
        {
            _webSocket = new ClientWebSocket();
            _cts = new CancellationTokenSource();
        }

        public void SetCompression(bool enabled)
        {
            _compressionStatus = enabled ? CompressionStatus.ON : CompressionStatus.OFF;
        }

        public async Task ConnectAsync(string host, int port, bool useSSL, string userId,  string apiKey)
        {
            
            // Input validation
            if (string.IsNullOrWhiteSpace(host))
            {
                OnError?.Invoke("Host cannot be null or empty.");
                return;
            }

            if (port <= 0 || port > 65535)
            {
                OnError?.Invoke("Port must be between 1 and 65535.");
                return;
            }

            if (string.IsNullOrWhiteSpace(userId))
            {
                OnError?.Invoke("User ID cannot be null or empty.");
                return;
            }

            if (!Uri.CheckHostName(host).Equals(UriHostNameType.Dns) &&
                !Uri.CheckHostName(host).Equals(UriHostNameType.IPv4) &&
                !Uri.CheckHostName(host).Equals(UriHostNameType.IPv6))
            {
                OnError?.Invoke("Host must be a valid hostname or IP address.");
                return;
            }

            // Optionally validate SSL usage
            //if (useSSL && port == 80)
            //{
            //    OnError?.Invoke("Port 80 is not valid for SSL connections. Use port 443 or another SSL-enabled port.");
            //    return;
            //}

            _userId = userId;
            string protocol = useSSL ? "wss" : "ws";
            string url = $"{protocol}://{host}:{port}";

            try
            {
                await _webSocket.ConnectAsync(new Uri(url), _cts.Token);

                Console.WriteLine("Connected");

                // Start receiving messages
                _ = Task.Run(() => ReceiveMessagesAsync());

                string currentTime = DateTime.Now.ToString("HH:mm:ss");

                string password = "68=";
                if (apiKey != null && !string.IsNullOrEmpty(apiKey.Trim()))
                {
                    password = string.Format("68=%s|401=2", apiKey);
                }

                // Build login message
                string loginMsg = $"63=FT3.0|64=101|65=74|66={currentTime}|67={_userId}|{password }";
                await SendMessageAsync(loginMsg);

                OnOpen?.Invoke();

            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Connection failed: {ex.Message}");
                //throw;
            }
        }

        public async Task DisconnectAsync()
        {
            if (_webSocket.State == WebSocketState.Open)
            {
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
            }
        }

        private async Task SendMessageAsync(string message)
        {
            if (_webSocket.State != WebSocketState.Open)
            {
                throw new InvalidOperationException("WebSocket is not connected");
            }
            Console.WriteLine("Sending Message: " + message);
            byte[] packet = fragHandler.FragmentData(System.Text.Encoding.ASCII.GetBytes(message));
            ArraySegment<byte> bufferSegment = new ArraySegment<byte>(packet);
            await _webSocket.SendAsync(bufferSegment, WebSocketMessageType.Binary, true, CancellationToken.None);
        }

        /// <summary>
        /// Send Touchline request for market data
        /// </summary>
        /// <param name="tokenList">List Token to subscribe (1_22,1_2885)</param>
        /// <param name="responseType">1 = Touchline with fixed length native data (refer response structure in section 4.5.3.5) 0 = Normal touchline(refer response structure in section 4.5.3.4)</param>
        /// <param name="LTPChangeOnly"> Send Touchline Response on LTP change if true, otherwise send all market data response</param>
        public async Task SubscribeTouchlineAsync(IEnumerable<string> tokenList, string responseType = "0", bool LTPChangeOnly =false)
        {
            if (tokenList == null || !tokenList.Any())
            {
                OnError?.Invoke("Token list cannot be null or empty.");
                return;
            }

            if (responseType != "0" && responseType != "1")
            {
                OnError?.Invoke("Invalid response type passed. Valid values are 0 or 1");
                return;
            }

            string strTokenToSubscribe = "";

            foreach (var item in tokenList)
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;

                String[] parts = item.Split('_');

                if (parts.Length != 2 ||
                    !int.TryParse(parts[0], out int marketSegmentId) ||
                    !int.TryParse(parts[1], out int token))
                {
                    OnError?.Invoke($"Invalid token format: '{item}'. Expected format: 'MarketSegmentID_Token'.");
                    continue;
                }

                strTokenToSubscribe += $"1={marketSegmentId}$7={token}|";
            }

            string strResposeType = "";
            if (responseType == "1")
            {
                strResposeType = "49=1";
            }

            string sLTChangeOnly = "200=0";
            if (LTPChangeOnly)
            {
                sLTChangeOnly = "200=1";
            }


            if (!string.IsNullOrEmpty(strTokenToSubscribe))
               
           {
                string tlRequest = "";

                if (strResposeType != "")
                {
                     tlRequest = $"63=FT3.0|64=206|65=84|66={DateTime.Now:HH:mm:ss}|{strResposeType}|{sLTChangeOnly}|{strTokenToSubscribe}230=1";
                }
                else
                {
                    tlRequest = $"63=FT3.0|64=206|65=84|66={DateTime.Now:HH:mm:ss}|{sLTChangeOnly}|{strTokenToSubscribe}230=1";
                }

                    await SendMessageAsync(tlRequest);

                Console.WriteLine("Subscribed to touchline tokens: " + string.Join(", ", tokenList));
            }
            else
            {
                OnError?.Invoke("No valid tokens found to subscribe.");
            }
        }

        public async Task UnsubscribeTouchlineAsync(IEnumerable<string> tokenList)
        {
            if (tokenList == null || !tokenList.Any())
            {
                OnError?.Invoke("Token list cannot be null or empty.");
                return;
            }

            string strTokenToSubscribe = "";

            foreach (var item in tokenList)
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;

                var parts = item.Split('_');

                if (parts.Length != 2 ||
                    !int.TryParse(parts[0], out int marketSegmentId) ||
                    !int.TryParse(parts[1], out int token))
                {
                    OnError?.Invoke($"Invalid token format: '{item}'. Expected format: 'MarketSegmentID_Token'.");
                    continue;
                }

                strTokenToSubscribe += $"1={marketSegmentId}$7={token}|";
            }

            if (!string.IsNullOrEmpty(strTokenToSubscribe))
            {
                string tlRequest =
                    $"63=FT3.0|64=206|65=84|66={DateTime.Now:HH:mm:ss}|4=|{strTokenToSubscribe}230=2";

                await SendMessageAsync(tlRequest);

                Console.WriteLine("Unsubscribed to touchline tokens: " + string.Join(", ", tokenList));
            }
            else
            {
                OnError?.Invoke("No valid tokens found to subscribe.");
            }
        }

        /// <summary>
        /// Subscribe to Market Depth for the provided token and market segment.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="MarketSegmentId"></param>
        /// <returns></returns>
        public async Task SubscribeBestFiveAsync(string token, int marketSegmentId)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                OnError?.Invoke("token cannot be null or empty.");
                return;
            }

            if (marketSegmentId <= 0)
            {
                OnError?.Invoke("Invalid MarketSegment.");
                return;
            }

            //string strTokenToSubscribe = "";
            string tlRequest = $"63=FT3.0|64=127|65=84|66={DateTime.Now:HH:mm:ss}|1={marketSegmentId}|7={token}|230=1";
            await SendMessageAsync(tlRequest);

            Console.WriteLine("Subscribed to BestFive tokens: " + token + " , MarketSegmentId: " + marketSegmentId);
        }

        public async Task UnsubscribeBestFiveAsync(string token, int marketSegmentId)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                OnError?.Invoke("token cannot be null or empty.");
                return;
            }

            if (marketSegmentId <= 0)
            {
                OnError?.Invoke("Invalid MarketSegment.");
                return;
            }

            //string strTokenToSubscribe = "";
            string tlRequest = $"63=FT3.0|64=127|65=84|66={DateTime.Now:HH:mm:ss}|1={marketSegmentId}|7={token}|230=2";
            await SendMessageAsync(tlRequest);

            Console.WriteLine("Unsubscribed to BestFive tokens: " + token + " , MarketSegmentId: " + marketSegmentId);
        }

        /// <summary>
        /// Send LTP Touchline request for market data
        /// </summary>
        /// <param name="tokenList">List Token to subscribe (1_22,1_2885)</param>
        public async Task SubscribeLTPTouchlineAsync(IEnumerable<string> tokenList)
        {
            if (tokenList == null || !tokenList.Any())
            {
                OnError?.Invoke("Token list cannot be null or empty.");
                return;
            }
           

            string strTokenToSubscribe = "";

            foreach (var item in tokenList)
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;

                var parts = item.Split('_');

                if (parts.Length != 2 ||
                    !int.TryParse(parts[0], out int marketSegmentId) ||
                    !int.TryParse(parts[1], out int token))
                {
                    OnError?.Invoke($"Invalid token format: '{item}'. Expected format: 'MarketSegmentID_Token'.");
                    continue;
                }

                strTokenToSubscribe += $"1={marketSegmentId}$7={token}|";
            }

            if (!string.IsNullOrEmpty(strTokenToSubscribe))
            {
                string tlRequest = $"63=FT3.0|64=347|65=84|66={DateTime.Now:HH:mm:ss}|{strTokenToSubscribe}230=1";

                await SendMessageAsync(tlRequest);

                Console.WriteLine("Subscribed to LTP touchline tokens: " + string.Join(", ", tokenList));
            }
            else
            {
                OnError?.Invoke("No valid tokens found to subscribe.");
            }
        }

        public async Task UnSubscribeLTPTouchlineAsync(IEnumerable<string> tokenList)
        {
            if (tokenList == null || !tokenList.Any())
            {
                OnError?.Invoke("Token list cannot be null or empty.");
                return;
            }


            string strTokenToSubscribe = "";

            foreach (var item in tokenList)
            {
                if (string.IsNullOrWhiteSpace(item))
                    continue;

                var parts = item.Split('_');

                if (parts.Length != 2 ||
                    !int.TryParse(parts[0], out int marketSegmentId) ||
                    !int.TryParse(parts[1], out int token))
                {
                    OnError?.Invoke($"Invalid token format: '{item}'. Expected format: 'MarketSegmentID_Token'.");
                    continue;
                }

                strTokenToSubscribe += $"1={marketSegmentId}$7={token}|";
            }

            if (!string.IsNullOrEmpty(strTokenToSubscribe))
            {
                string tlRequest = $"63=FT3.0|64=347|65=84|66={DateTime.Now:HH:mm:ss}|{strTokenToSubscribe}230=2";

                await SendMessageAsync(tlRequest);

                Console.WriteLine("Unsubscribed to LTP touchline tokens: " + string.Join(", ", tokenList));
            }
            else
            {
                OnError?.Invoke("No valid tokens found to subscribe.");
            }
        }

        /// <summary>
        /// This method can be use to pause or resume the broadcast subscription for user when portal / app is in minimine mode or broadcast is not needed temporarily.
        /// </summary>
        /// <param name="isPause">true – Pause false – Resume</param>
        /// <returns></returns>
        public async Task SubscribePauseResumeAsync(bool isPause)
        {
           
            string sIsPause = "";
            if (isPause)
            {
                sIsPause = "230=1";
            }
            else
            {
                sIsPause = "230=2";
            }

            string tlRequest = $"63=FT3.0|64=106|65=84|66={DateTime.Now:HH:mm:ss}|{sIsPause}";
            await SendMessageAsync(tlRequest);
            Console.WriteLine(isPause ? "Pause " : "Resume " +  "request Sent");

        }

        private async Task ReceiveMessagesAsync()
        {
            var loopToken = _cts.Token;
            MemoryStream outputStream = null;
            WebSocketReceiveResult receiveResult = null;
            var buffer = new byte[ReceiveBufferSize];
            ArraySegment<byte> bufferSegment = new ArraySegment<byte>(buffer);
            try
            {
                outputStream = new MemoryStream(ReceiveBufferSize);
                while (!loopToken.IsCancellationRequested)
                {
                    do
                    {
                        receiveResult = await _webSocket.ReceiveAsync(bufferSegment, _cts.Token);
                        if (receiveResult.MessageType != WebSocketMessageType.Close)
                            outputStream.Write(buffer, 0, receiveResult.Count);
                    }
                    while (!receiveResult.EndOfMessage);
                    if (receiveResult.MessageType == WebSocketMessageType.Close) break;
                    outputStream.Position = 0;
                    ResponseReceived(outputStream);
                    outputStream.SetLength(0);
                }
            }
            catch (TaskCanceledException tcExp)
            {
                Console.WriteLine("Error inTask: ", tcExp.Message);
                //OnClose?.Invoke(this, this);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error ReceiveLoop: ", ex.ToString());
                //OnClose?.Invoke(this, this);   
            }
            finally
            {
                outputStream?.Dispose();
            }

        }

        //byte[] globalPendingBytes;
        DateTime dteNSE = DateTime.Parse("01-01-1980");
        private void ResponseReceived(Stream inputStream)
        {
            try
            {
                byte[] bytRecvd = new byte[inputStream.Length];
                inputStream.Read(bytRecvd, 0, bytRecvd.Length);
                ArrayList arrData = fragHandler.DeFragment(bytRecvd);
                //ArrayList arrData = fragHandler.Defragmentation(bytRecvd,ref globalPendingBytes);
                for (int i = 0; i < arrData.Count; i++)
                {

                    string strMsg = Encoding.ASCII.GetString((byte[])arrData[i]);
                    if (strMsg.IndexOf("|50=") >= 0)
                    {
                        byte[] data = (byte[])arrData[i];
                        int dataIndex = strMsg.IndexOf("|50=") + 4;
                        string strNewMsg = strMsg.Substring(0, strMsg.IndexOf("|50=") + 1);
                        string MktSegId = BitConverter.ToInt32(data, dataIndex).ToString();
                        strNewMsg += "1=" + MktSegId + "|";
                        string token = BitConverter.ToInt32(data, dataIndex + 4).ToString();
                        strNewMsg += "7=" + token + "|";
                        string LUT = "";// BitConverter.ToInt32(data, dataIndex + 8).ToString();
                        LUT = dteNSE.AddSeconds(BitConverter.ToInt32(data, dataIndex + 8)).ToString("yyyy-MM-dd HHmmss");
                        strNewMsg += "74=" + LUT + "|";
                        string LTT = "";// BitConverter.ToInt32(data, dataIndex + 12).ToString();
                        LTT = dteNSE.AddSeconds(BitConverter.ToInt32(data, dataIndex + 12)).ToString("yyyy-MM-dd HHmmss");
                        strNewMsg += "73=" + LTT + "|";
                        string LTP = BitConverter.ToInt32(data, dataIndex + 16).ToString();
                        strNewMsg += "8=" + LTP + "|";
                        string BQty = BitConverter.ToInt32(data, dataIndex + 20).ToString();
                        strNewMsg += "2=" + BQty + "|";
                        string BPrice = BitConverter.ToInt32(data, dataIndex + 24).ToString();
                        strNewMsg += "3=" + BPrice + "|";
                        string SQty = BitConverter.ToInt32(data, dataIndex + 28).ToString();
                        strNewMsg += "5=" + SQty + "|";
                        string SPrice = BitConverter.ToInt32(data, dataIndex + 32).ToString();
                        strNewMsg += "6=" + SPrice + "|";
                        string OPrice = BitConverter.ToInt32(data, dataIndex + 36).ToString();
                        strNewMsg += "75=" + OPrice + "|";
                        string HPrice = BitConverter.ToInt32(data, dataIndex + 40).ToString();
                        strNewMsg += "77=" + HPrice + "|";
                        string LPrice = BitConverter.ToInt32(data, dataIndex + 44).ToString();
                        strNewMsg += "78=" + LPrice + "|";
                        string CPrice = BitConverter.ToInt32(data, dataIndex + 48).ToString();
                        strNewMsg += "76=" + CPrice + "|";
                        string DecLocator = BitConverter.ToInt32(data, dataIndex + 52).ToString();
                        strNewMsg += "399=" + DecLocator + "|";
                        string PrvClosePrice = BitConverter.ToInt32(data, dataIndex + 56).ToString();
                        strNewMsg += "250=" + PrvClosePrice + "|";
                        string IndicativeClosePrice = BitConverter.ToInt32(data, dataIndex + 60).ToString();
                        strNewMsg += "88=" + IndicativeClosePrice + "|";
                        strMsg = strNewMsg;
                    }
                    
                    OnMessage?.Invoke(strMsg);
                    //List<string> arrMsg = parseData(Encoding.ASCII.GetString((byte[])arrData[i]));
                    //console.log(message);
                    //foreach (string str in arrMsg)
                    //{
                    //    if (str.IndexOf("|50=") >= 0)
                    //    {

                    //    }
                    //    OnMessage?.Invoke(str);
                    //}
                }
                //Console.WriteLine(Encoding.ASCII.GetString((byte[])arrData[i]));

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing data: ", ex.Message);
                OnError?.Invoke("Error parsing data: " + ex.Message);
            }

        }

        private List<string> parseData(string data)
        {
            //this.buffer += data;
            var messages = new List<string>();
            while (data.Length > 6)
            {
                // Extract length from first 5 characters
                string lengthStr = data.Substring(1, 5);
                int messageLength = Convert.ToInt16(lengthStr);
                //console.log(lengthStr,messageLength);
                // Check if we have a complete message
                int totalLength = 6 + messageLength;
                //break;
                if (data.Length < totalLength)
                {
                    // Not enough data yet, wait for more
                    break;
                }

                // Extract the complete message
                //string fullMessage = data.Substring(1, totalLength);
                string messageData = data.Substring(6, totalLength - 6);
                
                //console.log(messageData);
                messages.Add(messageData);

                // Remove processed message from buffer
                data = data.Substring(totalLength);

            }

            return messages;
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _cts?.Cancel();
                _webSocket?.Dispose();
                _cts?.Dispose();
                _isDisposed = true;
            }
        }
    }
}
