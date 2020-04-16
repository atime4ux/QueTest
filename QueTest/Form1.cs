using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace QueTest
{
    public partial class Form1 : Form
    {
        libCommon.clsUtil objUtil = new libCommon.clsUtil();
        System.Threading.Thread t1;

        string qPath;
        bool start;

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            qPath = ".\\private$\\myTest";
            start = false;

            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
        }

        void Application_ApplicationExit(object sender, EventArgs e)
        {
            if (t1 != null)
                t1.Abort();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (start)
                start = false;
            else
                start = true;

            t1 = new System.Threading.Thread(new System.Threading.ThreadStart(runTest));
            DialogResult DR = MessageBox.Show("실행", "주의", MessageBoxButtons.OKCancel);
            if (DR == System.Windows.Forms.DialogResult.OK)
                t1.Start();
        }
        private void runTest()
        {
            libMyUtil.clsMSMQ objMSMQ = new libMyUtil.clsMSMQ();
            if (objMSMQ.canReadQueue(qPath))
            {
                while (start)
                {
                    object rcvData = objMSMQ.receiveData(qPath, new Type[] { typeof(long) }, 5);
                    if (rcvData != null)
                    {
                        long rcvNum = (long)rcvData;
                        objUtil.writeLog(rcvNum.ToString());
                    }
                }
            }
            else
                MessageBox.Show("접근 불가");
        }
    }
}
