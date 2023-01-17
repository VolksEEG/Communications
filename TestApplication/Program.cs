using System;
using System.IO.Ports;
using VolksEEG.Communications;

namespace TestApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] ports = SerialPort.GetPortNames();

            if (0 == ports.Length)
            {
                Console.WriteLine("No ports detected!!");

                return;
            }

            Console.WriteLine("Select a Com port by entering the associated index.");
            Console.WriteLine("");

            for (int i = 0; i < ports.Length; ++i)
            {
                Console.WriteLine(i.ToString() + " : " + ports[i]);
            }

            int portIndex = -1;

            while (portIndex == -1)
            {
                string selectedPort = Console.ReadLine();

                if (Int32.TryParse(selectedPort, out int temp))
                {
                    if ((temp >= 0) && (temp < ports.Length))
                    {
                        portIndex = temp;
                    }
                    else
                    {
                        Console.WriteLine("Value is not an index between 0 and " + (ports.Length - 1).ToString());
                    }
                }
                else
                {
                    Console.WriteLine("Value is not a number");
                }
            }
            
            ICommunicationsLink communicationsLink = new SerialLink(ports[portIndex]);
            VolksEegCommunications volksEegCommunications = new VolksEegCommunications(communicationsLink);

            volksEegCommunications.StartCommunications();

            volksEegCommunications.StartDataCapture();
        }
    }
}
