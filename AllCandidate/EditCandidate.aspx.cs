using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace AllCandidate
{
    public partial class EditCandidate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Populate cities dropdown list
                // You need to implement this method to fetch cities from the database
                // and bind them to the dropdownlist
                PopulateCitiesDropDown();
                if (Request.QueryString["id"] != null)
                {
                    int id;
                    if (int.TryParse(Request.QueryString["id"], out id))
                    {
                        PopulateFields(id);
                    }
                    else
                    {
                        // Handle invalid ID
                        // Redirect to the EditCandidate page with the ID in the query string
                        Response.Redirect("ListCandidates.aspx");

                    }
                }
                else
                {
                    // Handle missing ID
                    Response.Redirect("ListCandidates.aspx");
                }
            }
        }

     
        protected void UpdateCandidate(int id, string name, DateTime dob, int age, int cityId)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["CadidatedbConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string updateQuery = @"UPDATE candidate SET Candidate_Name = @name, DOB = @dob, Age = @age, City_Id = @city  WHERE Sr_No = @id";
                SqlCommand updateCommand = new SqlCommand(updateQuery, con);
                updateCommand.Parameters.AddWithValue("@id", id);
                updateCommand.Parameters.AddWithValue("@name", name);
                updateCommand.Parameters.AddWithValue("@dob", dob);
                updateCommand.Parameters.AddWithValue("@age", age);
                updateCommand.Parameters.AddWithValue("@city", cityId);

                con.Open();
                updateCommand.ExecuteNonQuery();
                Label successLabel = new Label();
                successLabel.Text = "Successfully updated candidate";
                form1.Controls.Add(successLabel);
                // Redirect to the list page after creating the candidate
                Response.Redirect("ListCandidates.aspx");
            }
        }
        protected void PopulateFields(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["CadidatedbConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT sr_no, candidate_name, dob, age, city_id FROM Candidate WHERE sr_no = @id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtName.Text = reader["candidate_name"].ToString();
                    txtDOB.Text = Convert.ToDateTime(reader["dob"]).ToString("yyyy-MM-dd");
                    txtAge.Text = reader["age"].ToString();

                    // You need to implement logic to populate the city field
                  ddlCity.SelectedValue = reader["city_id"].ToString(); // Assuming CityId property exists

                }
                else
                {
                    // Handle no records found
                }
            }
        }
        protected void PopulateCitiesDropDown()
        {

            string connectionString = ConfigurationManager.ConnectionStrings["CadidatedbConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "select * from [dbo].[City_Master]";

                SqlDataAdapter adapter = new SqlDataAdapter(query, con);

                DataTable dt = new DataTable();
                adapter.Fill(dt);
                ddlCity.DataSource = dt;
                ddlCity.DataValueField = "city_id";
                ddlCity.DataTextField = "city_name";
                ddlCity.DataBind();
            }
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            // Retrieve input values
            string name = txtName.Text;
            DateTime dob;
            int age;
            int cityId;

            // Attempt to parse values, handling potential nulls
            if (!DateTime.TryParse(txtDOB.Text, out dob))
            {

                dob = DateTime.MinValue; // Example default value
            }

            if (!int.TryParse(txtAge.Text, out age))
            {
                age = 0; // Example default value
            }

            if (!int.TryParse(ddlCity.SelectedValue, out cityId))
            {
                cityId = 0; // Example default value
            }

            // You need to implement this method to insert the new candidate into the database
            // using ADO.NET or Entity Framework

            try
            {

                if (Request.QueryString["id"] != null)
                {
                    int id;
                    if (int.TryParse(Request.QueryString["id"], out id))
                    {
                        UpdateCandidate(id,name, dob, age, cityId);
                    }
                    else
                    {
                        Label errorLabel = new Label();
                        errorLabel.Text = "Invalid candidate";
                        form1.Controls.Add(errorLabel);
                        // Response.Redirect("ListCandidates.aspx");
                    }
                }
                else
                {
                    Label errorLabel = new Label();
                    errorLabel.Text = "Error in updating candidate";
                    form1.Controls.Add(errorLabel);
                    // Handle missing ID
                    // Response.Redirect("ListCandidates.aspx");
                }
               
            }
            catch (Exception ex)
            {
                Label errorLabel = new Label();
                errorLabel.Text = "Error saving candidate: " + ex.Message;
                form1.Controls.Add(errorLabel);
            }
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {


            if (Request.QueryString["id"] != null)
            {
                int id;
                if (int.TryParse(Request.QueryString["id"], out id))
                {
                    PopulateFields(id);
                    string mainconn = ConfigurationManager.ConnectionStrings["CadidatedbConnectionString"].ConnectionString;
                    SqlConnection con = new SqlConnection(mainconn);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("delete FROM candidate where Sr_No='" + id + "'", con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Label successLabel = new Label();
                    successLabel.Text = "Successfully deleted candidate";
                    form1.Controls.Add(successLabel);
                    Response.Redirect("ListCandidates.aspx");

                }
                else
                {
                    // Handle invalid ID
                    Label errorLabel = new Label();
                    errorLabel.Text = "Invalid candidate";
                    form1.Controls.Add(errorLabel);

                }
            }
            else
            {
                // Handle missing ID
                Label errorLabel = new Label();
                errorLabel.Text = "Error in deleting candidate";
                form1.Controls.Add(errorLabel);
            }
          
        }
            protected void TbDobTextChanged(object sender, EventArgs e)
        {

            try
            {
                DateTime date = Convert.ToDateTime(txtDOB.Text);
                //TimeSpan timeSpan = System.DateTime.Now.Subtract(date);
                int year, month, days;
                month = 12 * (DateTime.Now.Year - date.Year) + (DateTime.Now.Month - date.Month);
                if (DateTime.Now.Day < date.Day)
                {
                    month -= 1;
                    //days = DateTime.DaysInMonth(date.Day, month) - date.Day + DateTime.Now.Day;
                }
                else
                {
                    days = DateTime.Now.Day - date.Day;
                }
                year = (month / 12);
                month -= year * 12;
                txtAge.Text = year.ToString();
            }
            catch
            {
                Exception exception = new Exception();
                txtAge.Text += exception.Message;
            }

        }
    }
}