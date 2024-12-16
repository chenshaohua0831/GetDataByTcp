using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using GetDataByTcp.BLL;
using GetDataByTcp.Model;
using Newtonsoft.Json;

class AsyncTcpServer
{
    private const int BufferSize = 1024;
    private TcpListener _tcpListener;
    private byte[] _buffer = new byte[BufferSize];
    private CancellationTokenSource _cancellationTokenSource;
    // 用于存储正在处理的客户端连接任务
    private List<Task> clientTasks = new List<Task>();

    public AsyncTcpServer(int port)
    {
        try
        {
            _tcpListener = new TcpListener(IPAddress.Any, port);
            _cancellationTokenSource = new CancellationTokenSource();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating TcpListener: {ex.Message}");
        }
    }

    public async Task Start()
    {
        try
        {
            _tcpListener.Start();
            Console.WriteLine("Server started. Waiting for connections...");
            // 立即开始接受下一个客户端连接
            _ = AcceptClients();
            // 处理已完成的客户端任务
            await ProcessCompletedTasks();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            _tcpListener.Stop();
        }
    }

    private async Task AcceptClients()
    {
        try
        {
            while (true)
            {
                TcpClient client = await _tcpListener.AcceptTcpClientAsync();
                Console.WriteLine("Client connected.");
                var clientTask = HandleClient(client, _cancellationTokenSource.Token);
                clientTasks.Add(clientTask);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error accepting client: {ex.Message}");
        }
    }

    private async Task ProcessCompletedTasks()
    {
        while (true)
        {
            if (clientTasks.Count > 0)
            {
                // 等待任意一个任务完成
                var completedTask = await Task.WhenAny(clientTasks);
                clientTasks.Remove(completedTask);
            }
        }
    }

    private async Task HandleClient(TcpClient client, CancellationToken cancellationToken)
    {
        try
        {
            NetworkStream stream = client.GetStream();
            string data = "";
            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                int bytesRead = await stream.ReadAsync(_buffer, 0, BufferSize);
                if (bytesRead == 0)
                {
                    // 客户端关闭了连接
                    break;
                }
                
                data += Encoding.UTF8.GetString(_buffer, 0, bytesRead);
                Console.WriteLine($"Received: {data}");

                //Air_5m_Model model = JsonConvert.DeserializeObject<Air_5m_Model>(data);


                //Air_5m_BLL testBLL = new Air_5m_BLL();
                //testBLL.Insert_Air_5m(model);


                string response = $"You sent: {data}";
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Client handling cancelled.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error handling client: {ex.Message}");
        }
        finally
        {
            client.Close();
        }
    }
}