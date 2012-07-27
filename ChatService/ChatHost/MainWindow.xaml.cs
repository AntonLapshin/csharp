using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Windows;
using ChatWcfService;

namespace ChatHost
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        ServiceHost _host;

        private void ButtonStartClick(object sender,
                                       RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(textBoxPort.Text))
            {
                MessageBox.Show("Порт не заполнен!");
                return;
            }

            buttonStart.IsEnabled = false;

            //Define base addresses so all 
            //endPoints can go under it

            CreateHost(new ChatServiceHostFactory());

            AddNetTcpBinding();

            AddServiceMetadataBehavior();

            OpenHost();
        }

        private void AddServiceMetadataBehavior()
        {
            //Define Metadata endPoint, So we can 
            //publish information about the service
            var behavior =
                new ServiceMetadataBehavior();
            _host.Description.Behaviors.Add(behavior);

            _host.AddServiceEndpoint(typeof (IMetadataExchange),
                                     MetadataExchangeBindings.CreateMexTcpBinding(),
                                     "net.tcp://" + cmdBoxIP.SelectedItem + ":" +
                                     (int.Parse(textBoxPort.Text) - 1) +
                                     "/Chat/mex");
        }

        private void AddNetTcpBinding()
        {
            var tcpBinding =
                new NetTcpBinding(SecurityMode.None, true) {MaxConnections = 100};


            //To maxmize MaxConnections you have 
            //to assign another port for mex endpoint

            //and configure ServiceThrottling as well
            var throttle = _host.Description.Behaviors.Find<ServiceThrottlingBehavior>();
            if (throttle == null)
            {
                throttle = new ServiceThrottlingBehavior {MaxConcurrentCalls = 100, MaxConcurrentSessions = 100};
                _host.Description.Behaviors.Add(throttle);
            }


            //Enable reliable session and keep 
            //the connection alive for 20 hours.
            tcpBinding.ReceiveTimeout = new TimeSpan(20, 0, 0);
            tcpBinding.ReliableSession.Enabled = true;
            tcpBinding.ReliableSession.InactivityTimeout =
                new TimeSpan(20, 0, 10);

            _host.AddServiceEndpoint(typeof (IChat),
                                     tcpBinding, "tcp");
        }

        private void OpenHost()
        {
            try
            {
                _host.Open();
            }
            catch (Exception ex)
            {
                textStatus.Text = ex.Message;
            }
            finally
            {
                if (_host.State == CommunicationState.Opened)
                {
                    textStatus.Text = "Opened";
                    buttonStop.IsEnabled = true;
                }
            }
        }

        private void CreateHost(ServiceHostFactory serviceHostFactory)
        {
            var tcpAdrs = GetTcpAddress();
            _host = serviceHostFactory.CreateServiceHost(typeof (ChatService), new[] {tcpAdrs});
            //_host = new ServiceHost(
            //  typeof(ChatService), new[] { tcpAdrs });
        }

        private Uri GetTcpAddress()
        {
            var tcpAdrs = new Uri("net.tcp://" +
                                  cmdBoxIP.SelectedItem + ":" +
                                  textBoxPort.Text + "/Chat/");
            return tcpAdrs;
        }

        private void ButtonStopClick(object sender, RoutedEventArgs e)
        {
            if (_host == null) return;
            CloseHost();
        }

        private void CloseHost()
        {
            try
            {
                _host.Close();
            }
            catch (Exception ex)
            {
                textStatus.Text = ex.Message;
            }
            finally
            {
                if (_host.State == CommunicationState.Closed)
                {
                    textStatus.Text = "Closed";
                    buttonStart.IsEnabled = true;
                    buttonStop.IsEnabled = false;
                }
            }
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            var strHostName = Dns.GetHostName();
            var iphostentry = Dns.GetHostEntry(strHostName);
            cmdBoxIP.ItemsSource = iphostentry.AddressList;
        }
    }
}
