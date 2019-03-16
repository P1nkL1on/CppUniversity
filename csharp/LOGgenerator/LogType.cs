using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGgenerator
{
    public static class LogConsts
    {
        static Random rnd = new Random();
        static string[] months = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        static string[] methods = new string[] { "GET", "POST", "ADD", "PUSH", "LOL", "ERROR", "PEACE","SUCCESS" };
        static string[] adresses = new string[] { "/foo/", "/fo/", "/done/", "/serv/", "/tmp/", "/clnt/" };
        static string nameCan = "1234567890qwertyuiopasdfghjklzxcvbnm";
        static string[] backs = new string[] { ".ru", ".com", ".html", ".en" };
        static string[] tags = new string[] { "cont", "temp", "time", "date", "cs", "vsu", "keka", "tutel", "prohor", "kodgen", "unity", "utility", "process", "java", "is", "so", "cool", "cpp" };

        public static string getAdress()
        {
            string res = adresses[rnd.Next(adresses.Length)];
            int leng = rnd.Next(3, 20);
            while (leng-- > 0) res += nameCan[rnd.Next(nameCan.Length)];
            res += backs[rnd.Next(backs.Length)];
            return res;
        }

        public static List<String> gethistory(int length)
        {
            List<String> res = new List<string>();
            if (length < 0)
                length = rnd.Next(5, 100);
            int time = 0;
            for (int i = 0; i < length; i++)
            {
                time += rnd.Next(20, 60 * 10);
                res.Add((time / 60 / 60 + ":" + (time / 60 % 60) + ":" + (time % 60) + " " + methods[rnd.Next(methods.Length)]));
            }
            return res;
        }

        public static string getVersion()
        {
            return String.Format("#Type: {0}", methods[rnd.Next(methods.Length)].ToLower());
        }
        public static string getDate()
        {
            return String.Format("#Date: {2}-{1}-{0} {3}:{4}:{5}", (rnd.Next(28) + 1).ToString().PadLeft(2, '0'), (rnd.Next(12) + 1).ToString().PadLeft(2, '0'), (1990 + rnd.Next(28)).ToString().PadLeft(4, '0'), rnd.Next(24).ToString().PadLeft(2, '0'), rnd.Next(60).ToString().PadLeft(2, '0'), rnd.Next(60).ToString().PadLeft(2, '0'));
        }
        public static string getFields()
        {
            String res = "";
            int length = rnd.Next(2, 10);
            for (int i = 0; i < length; i++)
            {
                res += " " + tags[rnd.Next(tags.Length)];
            }
            return String.Format("#Fields:{0}", res);
        }
        public static string makeRandomLog()
        {
            string result = getVersion() + "\r\n" + getDate() + "\r\n" + getFields() + "\r\n";
            string adress = getAdress();
            foreach (String line in gethistory(-1)){
                result += line + " " + adress + "\r\n";
            }
            return result;
        }
    }
}
