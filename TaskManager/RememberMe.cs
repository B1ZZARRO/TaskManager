using System.IO;
using System.Xml.Serialization;

namespace TaskManager;

public class RememberMe
{
    public static RememberMe GetRememberMe()
    {
        RememberMe rememberMe = null;
        string filename = Globals.RememberMeFile;
        if (File.Exists(filename))
        {
            using FileStream fs = new FileStream(filename, FileMode.Open);
            {
                XmlSerializer xser = new XmlSerializer(typeof(RememberMe));
                rememberMe = (RememberMe)xser.Deserialize(fs);
                fs.Close();
            }
        }
        else rememberMe = new RememberMe();
        return rememberMe;
    }
        
    public void Save()
    {
        string filename = Globals.RememberMeFile;
        if (File.Exists(filename)) File.Delete(filename);
        using FileStream fs = new FileStream(filename, FileMode.Create);
        {
            XmlSerializer xser = new XmlSerializer(typeof(RememberMe));
            xser.Serialize(fs, this);
            fs.Close();
        }
    }
        
    public string Login { get; set; }
    public string Password { get; set; }
    public bool Check { get; set; }
}