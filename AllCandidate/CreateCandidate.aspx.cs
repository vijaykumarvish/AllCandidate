using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AllCandidate
{
    public partial class CreateCandidate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CreateCandidateTable();
                // Populate cities dropdown list
                // You need to implement this method to fetch cities from the database
                // and bind them to the dropdownlist
                PopulateCitiesDropDown();
            }
        }
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            // Retrieve input values
            string name = txtName.Text;
            DateTime dob ;
            int age ;
            int cityId ;

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
                InsertCandidate(name, dob, age, cityId);
            }   catch ( Exception ex)
            {
                Label errorLabel = new Label();
                errorLabel.Text = "Error saving candidate: " + ex.Message;
                form1.Controls.Add(errorLabel);
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

        protected void CreateCandidateTable()
        {
            string mainconn = ConfigurationManager.ConnectionStrings["CadidatedbConnectionString"].ConnectionString;

            SqlConnection con = new SqlConnection(mainconn);
            con.Open();
            // create table 
            string checktablequery = "select * from information_schema.tables where table_name = 'candidate'";
            string createtablequery = @"
                create table candidate (
                    sr_no int identity(1, 1) primary key,
                    candidate_name varchar(255) not null,
                    dob date,
                    age int,
                    city_id int foreign key references city_master(city_id)
                );
            ";


            using (SqlCommand checktablecommand = new SqlCommand(checktablequery, con))

            using (SqlDataReader reader = checktablecommand.ExecuteReader())
            {
                if (!reader.HasRows) // check if reader has any rows (meaning table doesn't exist)
                {
                    // table doesn't exist, create it
                    SqlCommand createtablecommand = new SqlCommand(createtablequery, con);
                    createtablecommand.ExecuteNonQuery();
                }
                reader.Close();
            }

        }
        protected void InsertCandidate(string name, DateTime dob, int age, int cityId)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["CadidatedbConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Candidate (Candidate_Name, DOB, Age, City_Id) VALUES (@name, @dob, @age, @city)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@dob", dob);
                cmd.Parameters.AddWithValue("@age", age);
                cmd.Parameters.AddWithValue("@city", cityId);

                con.Open();
                cmd.ExecuteNonQuery();
                // Redirect to the list page after creating the candidate
                Response.Redirect("ListCandidates.aspx");
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