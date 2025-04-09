using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalRecSystem
{
    public class userInfo
    {
        public int id{ get; set; }
        public string name{ get; set; }
        public string password{ get; set; }
        public string pal{ get; set; }

        public userInfo(int id, string name, string password, string pal)
        {
            this.id = id;
            this.name = name;
            this.password = password;
            this.pal = pal;

        }
        public override string ToString()
        {
            return $"用户ID：{id}，" +
                $"用户昵称：{name}，" +
                $"用户密码：{password}，" +
                $"掌纹路径：{pal}";
        }
    }
}
