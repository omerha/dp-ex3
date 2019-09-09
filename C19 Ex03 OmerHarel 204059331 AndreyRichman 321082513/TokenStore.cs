using FacebookWrapper.ObjectModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace C19_Ex01_Omer_204059331_Andrey_321082513.sln
{
    class TokenStore
    {
        //Save Data
        public void SaveLogin(string dataToSave, string dataFileName)
        {
            string tempPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            tempPath = Path.Combine(tempPath, dataFileName + ".txt");

            //Convert To Json then to bytes
            using (Stream stream = new FileStream(tempPath, FileMode.CreateNew))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());
                xmlSerializer.Serialize(stream, this);
            }
        }
        //Load Data
        public User LoadLogin(string dataFileName)
        {
            string tempPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName, "data");
            tempPath = Path.Combine(tempPath, dataFileName + ".txt");
            User res = null;
            //Exit if Directory or File does not exist
            if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
            {
                using (Stream stream = new FileStream(tempPath, FileMode.Open))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(this.GetType());
                    res = xmlSerializer.Deserialize(stream) as User;
                }
            }
            return res;
        }
    }
}
