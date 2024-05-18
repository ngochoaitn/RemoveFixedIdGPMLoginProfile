using Newtonsoft.Json;
using System;
using System.IO;

namespace RemoveFixedIdGPMLoginProfile
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("Remove fixed id extension v115 GPM LOGIN");
            Console.WriteLine("========================================");
            Console.Write("Config profile path in tool GPM: ");
            string profilePathConfigOnToolGPM = Console.ReadLine();

            int total = 0;

            foreach (string profilePath in Directory.GetDirectories(profilePathConfigOnToolGPM))
            {
                string gpmDefinePath = Path.Combine(profilePath, "Default", "gpm_define");
                if (File.Exists(gpmDefinePath))
                {
                    string json = File.ReadAllText(gpmDefinePath);
                    dynamic dataProfile = JsonConvert.DeserializeObject<dynamic>(json);
                    dynamic json_data = JsonConvert.DeserializeObject<dynamic>(Convert.ToString(dataProfile.json_data));
                    string browserVersion = json_data.BrowseVersion;
                    //if (json.Contains("\\\"BrowseVersion\\\":\\\"115"))
                    if (browserVersion.StartsWith("115"))
                    {
                        string extensionPath = Path.Combine(profilePath, "Default", "GPMBrowserExtenions");
                        int count = 0;
                        foreach (string extPath in Directory.GetDirectories(extensionPath))
                        {
                            try
                            {
                                // Kiểm tra tệp "gpm_extension_fixed_id" trong thư mục hiện tại
                                string filePath = Path.Combine(extPath, "gpm_extension_fixed_id");
                                if (File.Exists(filePath))
                                {
                                    // Đổi tên tệp thành "gpm_extension_fixed_id_bak"
                                    string newFilePath = Path.Combine(extPath, "gpm_extension_fixed_id_bak");
                                    File.Move(filePath, newFilePath);
                                    count++;
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}", ConsoleColor.Red);
                            }
                        }
                        if (count > 0)
                        {
                            total++;
                            Console.WriteLine($"Change ext in profile: name={dataProfile?.name} (path={Path.GetFileName(profilePath)})", ConsoleColor.Green);
                        }
                    }
                }
            }
            Console.WriteLine($"============");
            Console.WriteLine($"Done. Remove fixed id {total} profile(s). Enter to quit.");
            Console.ReadLine();
        }
    }
}
