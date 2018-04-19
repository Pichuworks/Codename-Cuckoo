using Client.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Login lg = new Login();

            while(true)
            {
                lg.ShowDialog();
                if (lg.DialogResult == DialogResult.OK)
                {
                    Application.Run(new MainWindow(lg.main_id));
                    break;
                }
                else if (lg.DialogResult == DialogResult.Ignore)
                {
                    Signup sgp = new Signup();
                    sgp.ShowDialog();
                    if (sgp.DialogResult == DialogResult.OK)
                    {
                        continue;
                    }
                }
                else
                {
                    break;
                }
            }
            return;
        }
    }
}
