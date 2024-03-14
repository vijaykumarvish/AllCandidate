using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AllCandidate
{
    public partial class ListCandidates : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView();
            }
        }
        protected void btnCreateCandidate_Click(object sender, EventArgs e)
        {
            // Redirect to the "Create Candidate" page URL (replace with your actual URL)
            Response.Redirect("CreateCandidate.aspx");
        }

        protected void BindGridView()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CadidatedbConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                // Select query (assuming you want to include Age in the grid)
                string selectQuery = @" SELECT c.Sr_No, c.Candidate_Name, c.DOB, c.Age, cm.city_name AS City FROM candidate c INNER JOIN City_Master cm ON c.City_Id = cm.city_id";

                SqlDataAdapter sqladapter = new SqlDataAdapter(selectQuery, con);
                DataTable dt = new DataTable();
                sqladapter.Fill(dt);

                gvCandidates.DataSource = dt;
                gvCandidates.DataBind();
            }
        }
        protected void gvCandidates_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Get the ID of the selected row
            int candidateId = Convert.ToInt32(gvCandidates.DataKeys[e.NewEditIndex].Value);

            // Redirect to the EditCandidate page with the ID in the query string
            Response.Redirect($"EditCandidate.aspx?id={candidateId}");
        }

        protected void gvCandidates_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Get the ID of the selected row
            int candidateId = Convert.ToInt32(gvCandidates.DataKeys[e.RowIndex].Value);

            // Redirect to the EditCandidate page with the ID in the query string
            DeleteCandidate(candidateId);

        }

        protected void DeleteCandidate(int id)
        {
            string mainconn = ConfigurationManager.ConnectionStrings["CadidatedbConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(mainconn);
            con.Open();
            SqlCommand cmd = new SqlCommand("delete FROM candidate where Sr_No='" + id + "'", con);
            cmd.ExecuteNonQuery();
            con.Close();
            BindGridView();
        }

    }
}