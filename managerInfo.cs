using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalRecSystem
{
    class managerInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public string password { get; set; }

        public managerInfo(int id, string name, string password)
        {
            this.id = id;
            this.name = name;
            this.password = password;
        }
    }
}
