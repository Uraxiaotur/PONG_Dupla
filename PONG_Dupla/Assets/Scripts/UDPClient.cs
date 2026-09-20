using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Globalization;

public class UdpClientWithId : MonoBehaviour {
    UdpClient client;
    Thread receiveThread;
    IPEndPoint serverEP;
    int myId = -1;

    public GameObject localCube;
    public GameObject localBall;

    void Start() {
        client = new UdpClient();
        serverEP = new IPEndPoint(IPAddress.Parse("10.0.37.146"), 5001);
        client.Connect(serverEP);

        // Thread para ouvir respostas do servidor
        receiveThread = new Thread(ReceiveData);
        receiveThread.Start();

        // Envia mensagem inicial para o servidor
        byte[] hello = Encoding.UTF8.GetBytes("HELLO");
        client.Send(hello, hello.Length);
    }
    void Update() {
        // Movimenta o cubo local
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        localCube.transform.Translate(new Vector3(0, v, 0) * (Time.deltaTime * 5));

        // Envia posição formatada
        string msg = "POS:" + 
                     localCube.transform.position.x.ToString("F2", CultureInfo.InvariantCulture) + ";" + 
                     localCube.transform.position.y.ToString("F2", CultureInfo.InvariantCulture);
        
        string msgBall = null;
        
        if (localBall && myId == 1)
        {
            msgBall = "BPOS:" + 
                      localBall.transform.position.x.ToString("F2", CultureInfo.InvariantCulture) + ";" + 
                      localBall.transform.position.y.ToString("F2", CultureInfo.InvariantCulture);
        }

        byte[] data = Encoding.UTF8.GetBytes(msg);
        byte[] dataBall = Encoding.UTF8.GetBytes(msgBall);
        client.Send(dataBall, dataBall.Length);
        client.Send(data, data.Length);
    }

    void ReceiveData() {
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
        while (true) {
            byte[] data = client.Receive(ref remoteEP);
            string msg = Encoding.UTF8.GetString(data);

            if (msg.StartsWith("ASSIGN:")) {
                myId = int.Parse(msg.Substring(7));
                Debug.Log("[Cliente] Recebi ID = " + myId);
            }
            
            byte[] dataBall = client.Receive(ref remoteEP);
            string msgBall = Encoding.UTF8.GetString(dataBall);

            if (myId != 1 && msgBall.StartsWith("BPOS:"))
            {
                string coords = msgBall.Substring(5); // remove "BPOS:"
                string[] parts = coords.Split(';');
                if (parts.Length == 2)
                {
                    float x = float.Parse(parts[0], System.Globalization.CultureInfo.InvariantCulture);
                    float y = float.Parse(parts[1], System.Globalization.CultureInfo.InvariantCulture);
                    localBall.transform.position = Vector2.MoveTowards(localBall.transform.position, new Vector2(x, y), 
                        Time.deltaTime * 5);
                }
            }
        }
    }

    void OnApplicationQuit() {
        byte[] quitMsg = Encoding.UTF8.GetBytes($"QUIT; {myId}");
        client.Send(quitMsg, quitMsg.Length);
        
        receiveThread.Abort();
        client.Close();
    }
}