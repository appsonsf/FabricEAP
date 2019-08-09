using IdentityModel.Client;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading;
using Newtonsoft.Json;
using MassTransit;
using Base.Eap.Notify.MsgContracts;

namespace ClientEmulator
{
    public partial class MainForm : Form
    {

        private string UploadFilePath;
        private string DownloadPath;
        SynchronizationContext m_SyncContext = null;
        public MainForm()
        {
            InitializeComponent();
            m_SyncContext = SynchronizationContext.Current;
            Load += MainForm_Load;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var folder = Directory.GetCurrentDirectory();
            UploadFilePath = Directory.CreateDirectory(folder).Parent.Parent.FullName;
            DownloadPath = UploadFilePath;
            textLocation.Text = UploadFilePath + "\\icon_master.png";
            textGroupId.Text = "59624bf1-6566-4a5a-8b6f-a65fe2cc3f2e";
            TextFileId.Text = "7dab0af0-2e04-49bf-84ca-44113ae8d660";
            textBaseAddress.Text = "http://localhost:10121/api/groupfile";
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            JsonConvert.DefaultSettings = () =>
            {
                var setting = new JsonSerializerSettings();
                setting.Formatting = Formatting.Indented;
                return setting;
            };
        }

        private string accessToken;
        private async void btnLogin_ClickAsync(object sender, EventArgs e)
        {
            var response = await RequestTokenAsync(txtAuthority.Text, txtUsername.Text, txtPassword.Text);
            response.Show();
            if (!response.IsError)
            {
                accessToken = response.AccessToken;
                txtAccessToken.Text = accessToken;
                MessageBox.Show("登录成功");
            }
            else
                MessageBox.Show("登录失败： " + response.Error);
        }

        #region Utility
        static async Task<TokenResponse> RequestTokenAsync(string authority, string username, string pwd)
        {
            var discoClient = new DiscoveryClient(authority)
            {
                Policy = new DiscoveryPolicy
                {
                    RequireHttps = false
                }
            };
            var disco = await discoClient.GetAsync();// await DiscoveryClient.GetAsync(Authority);
            if (disco.IsError) throw new Exception(disco.Error);

            var client = new TokenClient(
                disco.TokenEndpoint,
                "OM_TT_Desktop_001",
                "tt@scrbg");
            return await client.RequestResourceOwnerPasswordAsync(username, pwd,
                "openid phone profile profile.ext sso.sts eap.api");
        }
        #endregion

        private RestClient CreateRestClient()
        {
            var client = new RestClient(txtAuthority.Text);
            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(accessToken, "Bearer");
            return client;
        }

        private async void btnTempToken_Click(object sender, EventArgs e)
        {
            var client = CreateRestClient();

            var request = new RestRequest("api/TempToken", Method.GET);

            var response = await client.ExecuteTaskAsync<string>(request);
            if (response.IsSuccessful)
            {
                MessageBox.Show("请求成功");
                txtTempToken.Text = response.Data;
            }
            else
                MessageBox.Show($"请求失败： {response.ErrorMessage}, HttpStatusCode: {response.StatusCode} ");
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }


        private void buttonChoose_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "AllFiles|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    textLocation.Text = openFileDialog.FileName;
                }
            }
        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            var uploadUrl = textBaseAddress.Text.Trim() + "/upload";
            if (Guid.TryParse(textGroupId.Text.ToString(), out Guid groupId))
            {
                var param = new UploadParam()
                {
                    MD5 = Guid.NewGuid().ToString("N"),
                    GroupId = groupId,
                };
                var location = textLocation.Text.ToString();
                Task.Run(async () => await UploadFileAsync(uploadUrl, location, param));
            }
            else
            {
                UpdateUploadMsg("群组Id不正确");
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task<string> UploadFileAsync(string url, string path, UploadParam fileParam)
        {
            Debug.WriteLine("---------------------------------------");
            Debug.WriteLine("URL = " + url);
            //Debug.WriteLine("Content = " + JsonConvert.SerializeObject(fileParam));
            Debug.WriteLine("---------------------------------------");
            var uploadMessage = "";
            try
            {
                using (var client = new HttpClient())
                {
                    client.SetBearerToken(accessToken);
                    using (var content = new MultipartFormDataContent("Upload----" + DateTime.Now.Ticks.ToString("x")))
                    {
                        using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {

                            content.Add(new StringContent(fileParam.GroupId.ToString()), "GroupId");
                            content.Add(new StringContent(fileParam.MD5.ToString()), "MD5");
                            var streamContent = new StreamContent(stream);
                            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                            streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                            {
                                Name = "file", //上传文件的name标识,用于action参数构造
                                FileName = Path.GetFileName(path), //上传的文件的名称
                            };
                            content.Add(streamContent);
                            try
                            {
                                client.DefaultRequestHeaders.Accept.Clear();
                                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                                client.DefaultRequestHeaders
                                    .Accept
                                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                client.DefaultRequestHeaders.Connection.Add("Keep-Alive");
                                using (var httpResponseMessage = await client.PostAsync(url, content))
                                {
                                    var responseContent = "";
                                    if (httpResponseMessage.IsSuccessStatusCode)
                                    {
                                        responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                                        uploadMessage = " UploadFileAsync " + httpResponseMessage.StatusCode + "\n" + responseContent;
                                    }
                                    else
                                    {
                                        uploadMessage = " UploadFileAsync Failed " + httpResponseMessage.StatusCode;
                                    }
                                    Debug.WriteLine(uploadMessage);
                                    UpdateUploadMsg(uploadMessage);
                                    return responseContent;
                                }
                            }
                            catch (Exception e)
                            {
                                uploadMessage = "UploadFileAsync Exception === " + e.Message;
                                Debug.WriteLine(uploadMessage);
                                UpdateUploadMsg(uploadMessage);
                                return "";
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                uploadMessage = "UploadFileAsync Exception === " + e.Message;
                Debug.WriteLine(uploadMessage);
                UpdateUploadMsg(uploadMessage);
                return "";
            }
        }

        private void UpdateUploadMsg(string msg)
        {
            m_SyncContext.Post((message) =>
            {
                TextUploadMsg.Text = msg;
            }, msg);
        }

        private void UpdateDownloadMsg(string msg)
        {
            m_SyncContext.Post((message) =>
            {
                TextDownloadMsg.Text = message.ToString();
            }, msg);
        }
        private void buttonDownload_Click(object sender, EventArgs e)
        {
            DownloadFile(TextFileId.Text);
        }

        private void DownloadFile(string fileId)
        {
            var filename = "download.png";
            var filePath = DownloadPath + "\\" + filename;
            var downloadUrl = textBaseAddress.Text.Trim() + "/download/{0}";

            try
            {
                using (var client = new HttpClient())
                {
                    try
                    {
                        FileStream fs;
                        client.SetBearerToken(accessToken);
                        var bytes = client.GetByteArrayAsync(string.Format(downloadUrl, fileId)).Result;

                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        fs = new System.IO.FileStream(filePath, System.IO.FileMode.CreateNew);
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Close();
                        if (File.Exists(filePath))
                        {
                            UpdateDownloadMsg(" download success " + filePath);
                        }
                        else
                        {
                            UpdateDownloadMsg(" download failed file not exist " + filePath);
                        }
                    }
                    catch (Exception e)
                    {
                        UpdateDownloadMsg(" download failed " + e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                UpdateDownloadMsg(" download failed " + e.Message);
            }
        }

        private void BtnAttachment_SelectFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "AllFiles|*.*";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtAttachment_FilePath.Text = openFileDialog.FileName;
                }
            }
        }

        private void BtnAttachment_UploadFile_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("---------------------------------------");
            var fakedMd5 = Guid.NewGuid().ToString();
            Debug.WriteLine("fakedMd5:" + fakedMd5);
            var uploadMessage = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(txtBaseAddress.Text);
                client.SetBearerToken(accessToken);
                using (var content = new MultipartFormDataContent("Upload----" + DateTime.Now.Ticks.ToString("x")))
                {
                    using (var stream = File.Open(txtAttachment_FilePath.Text, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        content.Add(new StringContent(txtAttachment_ContextId.Text), "ContextId");
                        content.Add(new StringContent(fakedMd5), "MD5");
                        var streamContent = new StreamContent(stream);
                        streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "file", //上传文件的name标识,用于action参数构造
                            FileName = Path.GetFileName(txtAttachment_FilePath.Text), //上传的文件的名称
                        };
                        content.Add(streamContent);
                        client.DefaultRequestHeaders.Accept.Clear();
                        //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        client.DefaultRequestHeaders
                            .Accept
                            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Connection.Add("Keep-Alive");

                        var task = Task.Run(async () =>
                        {
                            using (var httpResponseMessage = await client.PostAsync("attachment", content))
                            {
                                var responseContent = "";
                                if (httpResponseMessage.IsSuccessStatusCode)
                                {
                                    responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                                    uploadMessage = " UploadFileAsync " + httpResponseMessage.StatusCode + "\n" + responseContent;
                                }
                                else
                                {
                                    uploadMessage = " UploadFileAsync Failed " + httpResponseMessage.StatusCode;
                                }
                            }
                        });

                        Task.WaitAny(task);
                        MessageBox.Show(uploadMessage);
                    }
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var form = new IMChatForm();
            form.Show();
        }

        private IBusControl _bus;
        private void BtnConnectRabbitMq_Click(object sender, EventArgs e)
        {
            var address = txtRabbitMqAddress.Text.EndsWith("/") ? txtRabbitMqAddress.Text : txtRabbitMqAddress.Text + "/";
            var userName = txtRabbitMqUsername.Text;
            var passwrod = txtRabbitMqPassword.Text;

            if (string.IsNullOrEmpty(address) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passwrod))
            {
                MessageBox.Show("请输入正确的数据!");
                return;
            }

            try
            {
                _bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    var host = cfg.Host(new Uri(address), h =>
                    {
                        h.Username(userName);
                        h.Password(passwrod);
                    });
                });

                var endpointUri = new Uri(address + BizSystemNotifyMsg.RabbitMqReceiveEndpointName);

                EndpointConvention.Map<BizSystemNotifyMsg>(endpointUri);

                MessageBox.Show("连接成功");
            }
            catch (Exception ex)
            {
                Debug.Write(ex.ToString());
                MessageBox.Show("启动失败,请检查工作环境");
            }
        }

        private void BtnSendNotifyMsg_Click(object sender, EventArgs e)
        {
            _bus.Send(new BizSystemNotifyMsg
            {
                SystemId = txtNotifyBizSystemId.Text,
                Category = int.Parse(txtNotifyMsgCategory.Text),
                Content = txtNotifyMsgContent.Text,
                ReceiverUserIds = txtNotifyMsgUserIds.Text.Split(',').Select(x => new Guid(x)).ToArray()
            }) ;

            MessageBox.Show("发送成功");
        }
    }
}
