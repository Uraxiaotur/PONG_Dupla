using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Globalization;

public class UdpClientTwoClients : MonoBehaviour {
    UdpClient client;
    Thread receiveThread;
    IPEndPoint serverEP;
    int myId = -1;
    Vector3 remotePos = Vector3.zero;

    public GameObject localCube;
    public GameObject remoteCube;
    public GameObject localBall;

    void Start() {
        client = new UdpClient();
        serverEP = new IPEndPoint(IPAddress.Parse("10.57.1.50"), 5001);
        client.Connect(serverEP);
        receiveThread = new Thread(ReceiveData);
        receiveThread.Start();
        client.Send(Encoding.UTF8.GetBytes("HELLO"), 5);
        
        remotePos = remoteCube.transform.position;
    }
    void Update() {
        // Movimento local
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        localCube.transform.Translate(new Vector3(0, v, 0) * (Time.deltaTime * 5));

        // Envia posição
        string msg = "POS:" +
                     localCube.transform.position.x.ToString("F2", CultureInfo.InvariantCulture) + ";" +
                     localCube.transform.position.y.ToString("F2", CultureInfo.InvariantCulture);
        client.Send(Encoding.UTF8.GetBytes(msg), msg.Length);
        
        string msgBall = null;
        if (localBall && myId == 1)
        {
            msgBall = "BPOS:" +
                      localBall.transform.position.x.ToString("F2", CultureInfo.InvariantCulture) + ";" +
                      localBall.transform.position.y.ToString("F2", CultureInfo.InvariantCulture);
            client.Send(Encoding.UTF8.GetBytes(msgBall), msgBall.Length);
        }

        // Atualiza posição do outro jogador
        remoteCube.transform.position = Vector3.Lerp(remoteCube.transform.position, remotePos, Time.deltaTime * 10f
        );
    }
    void ReceiveData() {
        IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);
        
        while (true) {
            byte[] data = client.Receive(ref remoteEP);
            string msg = Encoding.UTF8.GetString(data);

            if (msg.StartsWith("ASSIGN:")) {
                myId = int.Parse(msg.Substring(7));
                Debug.Log("[Cliente] Meu ID = " + myId);
            }
            else if (msg.StartsWith("POS:")) {
                string[] parts = msg.Substring(4).Split(';');
                if (parts.Length == 3) {
                    int id = int.Parse(parts[0]);
                    if (id != myId) {
                        float x = float.Parse(parts[1], CultureInfo.InvariantCulture);
                        float y = float.Parse(parts[2], CultureInfo.InvariantCulture);
                        remotePos = new Vector3(-x, y, 0);
                        
                        
                    }
                }
            }
            else if (msg.StartsWith("BPOS:") && myId != 1)
            {
                string[] parts = msg.Substring(5).Split(';');
                float  x = float.Parse(parts[0], CultureInfo.InvariantCulture);
                float y = float.Parse(parts[1], CultureInfo.InvariantCulture);
                
                localBall.transform.position = new Vector3(x, y, 0);
            }

            if (msg.StartsWith("BPOS:") && myId != 1)
            {
                string[] parts = msg.Substring(5).Split(';');
                if (parts.Length == 2)
                {
                    int ballX = int.Parse(parts[0]);
                    int ballY = int.Parse(parts[1]);
                    localBall.transform.position = Vector3.Lerp(localBall.transform.position, new Vector3(-ballX,ballY , 0), Time.deltaTime * 10f);
                }
            }
        }
    }
    void OnApplicationQuit() {
        string quitMsg = $"QUIT: {myId}";
        client.Send(Encoding.UTF8.GetBytes(quitMsg), quitMsg.Length);
        
        receiveThread.Abort();
        client.Close();
    }
}