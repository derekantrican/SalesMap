using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SalesMap
{
    public partial class SalesMapSearch : Form
    {
        public SalesMapSearch()
        {
            InitializeComponent();

            //Eventual format: [][] [[REGION_NAME, IS_MULTIPART]]
            string[] regions = new string[] {"","(AL) Alabama", "(AK) Alaska", "(AZ) Arizona","(AR) Arkansas",
                                            "(BC) British Columbia",
                                            "(CA) California", "(CO) Colorado", "(CT) Conneticut",
                                            "(DE) Delaware", "(FL) Florida", "(GA) Georgia", "(HI) Hawaii",
                                            "(ID) Idaho", "(IL) Illinois", "(IN) Indiana", "(IA) Iowa",
                                            "(KS) Kansas", "(KY) Kentucky", "(LA) Louisiana", "(ME) Maine", "(MA) Massachusetts",
                                            "(MB) Manitoba",
                                            "(MI) Michigan", "(MN) Minnesota", "(MS) Mississippi", "(MO) Missouri", "(MT) Montana",
                                            "(NB) New Brunswick",
                                            "(NE) Nebraska", "(NV) Nevada", "(NH) New Hampshire", "(NJ) New Jersey", "(NL) Newfoundland and Labrador",
                                            "(NM) New Mexico",
                                            "(NY) New York", "(NC) North Carolina", "(ND) North Dakota", "(OH) Ohio", "(OK) Oklahoma",
                                            "(OR) Oregon", "(PA) Pennsylvania", "(RI) Rhode Island", "(SC) South Carolina", "(SD) South Dakota",
                                            "(TN) Tennessee", "(TX) Texas", "(UT) Utah", "(VT) Vermont", "(VA) Virginia", "(WA) Washington!",
                                            "(WV) West Virginia", "(WI) Wisconsin", "(WY) Wyoming"};

            //Eventual format: [][] [[REP_NAME,EMAIL,PHONE,RESPONSIBLE_REGION_INDEX]]
            string[] representatives = new string[] { "", "Brian Blair", "Arie Brown", "Matt Brubaker", "Donnie Dugger", "Jim Elmore", "Faber Fields",
                                                      "Jennifer Galigher", "Joaquin Gonzales", "Andrew Hood", "Dan Jacobs", "Larsen Kjellman",
                                                      "Scott Lindley", "Ryan Lustig", "Albert Otto", "Jonathan Padial", "David Sipho", "Ethan Swanson",
                                                      "Scott Taylor", "Steve Ties", "Schalk van Niekerk", "Ryan Weymouth", "Bryan Wurbarton"};

            comboBoxState.DataSource = regions;
            comboBoxRepresentative.DataSource = representatives;
        }

        private void comboBoxRepresentative_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxRepresentative.SelectedItem.ToString() != "")
            {
                comboBoxState.SelectedIndex = 0;
                labelRegionResult.Text = "Region: ";

                labelRepResult.Text = "Sales Rep: " + comboBoxRepresentative.SelectedItem.ToString();
                //labelContactResult.Text = "Contact: " + 
            }

        }

        private void comboBoxState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxState.SelectedItem.ToString() != "")
            {
                comboBoxRepresentative.SelectedIndex = 0;
                labelRepResult.Text = "Sales Rep: ";
                labelContactResult.Text = "Contact: ";

                labelRegionResult.Text = "Region: " + comboBoxState.SelectedItem.ToString();
            }
        }
    }
}
