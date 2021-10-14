using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.BZip2;
using System.IO.Compression;


namespace WindowsFormsApplication1
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }



        private void btnOper_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtFilePath.Text))
            {
                "Файл не найден {0}".ShowError(txtFilePath.Text);
                return;
            }
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(txtFilePath.Text);
            string xval = xdoc.get("Compressed");
            if (xval == "")
            {
                "Файл не содержит сжатых данных".ShowError();
                return;
            }
            byte[] data = Convert.FromBase64String(xval);
            MemoryStream stream = new MemoryStream(data);

            byte[] ret = null;
            ZipFile zf = new ZipFile(stream);
            ZipEntry ze = zf[0];

            if (ze == null)
            {
                "Ошибка распаковки файла".ShowError();
                return;
            }
            Stream s = zf.GetInputStream(ze);
            ret = new byte[ze.Size];
            s.Read(ret, 0, ret.Length);
            string sStrVal = System.Text.Encoding.UTF8.GetString(ret, 0, ret.Length);
            XmlDocument xdoc2 = new XmlDocument();
            xdoc2.LoadXml(sStrVal);
            XmlDocumentFragment xfrag = xdoc.CreateDocumentFragment();
            xfrag.InnerXml = xdoc2.InnerXml;
            xdoc.SelectSingleNode("//*[local-name()='Body']").RemoveAll();
            xdoc.SelectSingleNode("//*[local-name()='Body']").AppendChild(xfrag);
            xdoc.Save(txtFilePath.Text);
            "Документ успешно распакован".ShowInfo();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtFilePath.Text = openFileDialog1.FileName;
            }
        }
    }
}
