using System;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace compress_file
{
    public partial class frmCompressFile : Form
    {
        public frmCompressFile()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                
                if(openFileDialog.ShowDialog()==DialogResult.OK)
                {
                    txtFile.Text = openFileDialog.FileName;
                }
            }
            catch 
            {

                MessageBox.Show("An error has occurred", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAccion_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboType.SelectedIndex > -1)
                {
                    if (!string.IsNullOrEmpty(txtFile.Text.Trim()))
                    {
                        if (File.Exists(txtFile.Text.Trim()))
                        { 
                            Cursor.Current = Cursors.WaitCursor;
                            if (cboType.SelectedIndex == 1)
                            {
                                MemoryStream files = new MemoryStream();
                                using (FileStream file = new FileStream(txtFile.Text, FileMode.Open, FileAccess.Read))
                                {
                                    byte[] bytes = new byte[file.Length];
                                    file.Read(bytes, 0, (int)file.Length);
                                    files.Write(bytes, 0, (int)file.Length);
                                }
                                files.Position = 0;

                                using (var archive = new ZipArchive(files, ZipArchiveMode.Read, true))
                                {
                                    for (int i = 0; i <= archive.Entries.Count - 1; i++)
                                    {
                                        using (Stream streamZip = archive.Entries[i].Open())
                                        {
                                            using (MemoryStream streamFile = new MemoryStream())
                                            {
                                                string path = Path.GetDirectoryName(txtFile.Text);

                                                FileStream file = new FileStream($@"{path}\{archive.Entries[i].FullName}", FileMode.Create, FileAccess.Write);
                                                streamFile.WriteTo(file);
                                                file.Close();
                                            }
                                        }
                                    }
                                }

                                MessageBox.Show("decompress finished", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                var zipFile = $@"{Path.GetDirectoryName(txtFile.Text)}\{Path.GetFileNameWithoutExtension(txtFile.Text)}.zip";

                                if(File.Exists(zipFile))
                                { File.Delete(zipFile); }

                                using (var archive = ZipFile.Open(zipFile, ZipArchiveMode.Create))
                                {
                                    archive.CreateEntryFromFile(txtFile.Text, Path.GetFileName(txtFile.Text));
                                }

                                MessageBox.Show("Compress finished", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("File not exits", "Valitadion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select a file", "Valitadion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a Type", "Valitadion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch 
            {

                MessageBox.Show("An error has occurred", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void frmCompressFile_Load(object sender, EventArgs e)
        {

        }
    }
}
