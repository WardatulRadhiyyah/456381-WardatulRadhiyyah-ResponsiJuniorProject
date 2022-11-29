using System.Data;
using System.Xml.Linq;
using Npgsql;
using System.Windows.Forms;
namespace Responsi2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private NpgsqlConnection conn;
        string connstring = "Host =localhost;Port = 2022;Username = postgres; Password =informatika; Database = departemen ";
        public DataTable dt;
        public static NpgsqlCommand cmd;
        private string sql = null;
        private DataGridViewRow r;
        private void btnDelete_Click(object sender, EventArgs e)
        {
            {
                if (r == null)
                {
                    MessageBox.Show("Mohon pilih barus data yang akan diupdate", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (MessageBox.Show("Apakah benar anda ingin menghapus data " + r.Cells["_name"].Value.ToString() + " ?", "Hapus data terkonfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    try
                    {
                        conn.Open();
                        sql = @"select * from st_delete(:_id)";
                        cmd = new NpgsqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("_id_dep", r.Cells["_id_dep"].Value.ToString());
                        if ((int)cmd.ExecuteScalar() == 1)
                        {
                            MessageBox.Show("Data Users berhasil dihapus", "Well Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            conn.Close();
                            btnLoaddata.PerformClick();
                            txtNama.Text = cmbDep.Text  = null;
                            r = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error:" + ex.Message, "DELETE FAIL!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                sql = @"select * from stt_insert(:_nama,:_nama_dep)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_nama", txtNama.Text);
                cmd.Parameters.AddWithValue("_nama_dep", cmbDep.Text);
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Data Users Berhasil diinputkan", "Well done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    btnLoaddata.PerformClick();
                    txtNama.Text = cmbDep.Text  = null;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Insert FAIL!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoaddata_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                dgvData.DataSource = null;
                sql = @"select * from stt_select()";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                NpgsqlDataReader rd = cmd.ExecuteReader();
                dt.Load(rd);
                dgvData.DataSource = dt;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "FAIL!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Mohon pilih baris data yang akan diupdate", "Good!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                conn.Close();
                btnLoaddata.PerformClick();
                txtNama.Text = cmbDep.Text = null;
                return;
            }
            try
            {
                conn.Open();
                sql = @"select * from st_update(:_id, :_name, :_alamat, :_no_handphone)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_id_dep", r.Cells["_id_dep"].Value.ToString());
                cmd.Parameters.AddWithValue("_nama", txtNama.Text);
                cmd.Parameters.AddWithValue("_nama_dep", cmbDep.Text);
                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Data Users berhasil diupdate", "Well Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    btnLoaddata.PerformClick();
                    txtNama.Text = cmbDep.Text = null;
                    r = null;
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "UPDATE FAIL!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            {
                if (e.RowIndex >= 0)
                {
                    r = dgvData.Rows[e.RowIndex];
                    txtNama.Text = r.Cells["_nama"].Value.ToString();
                    cmbDep.Text = r.Cells["_nama_dep"].Value.ToString();
                   
                }
            }
        }
    }
}