using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ZamRecipeParser
{
    public partial class Form1 : Form
    {
        FormDatabase frmDatabase;
        List<Recipe> recipes = new List<Recipe>();
        public Form1()
        {
            InitializeComponent();
            frmDatabase = new FormDatabase();
        }

        private void DisplayRecipe(Recipe recipe)
        {
            txtName.Text = recipe.Name;
            txtBook.Text = recipe.Book;
            txtDevice.Text = recipe.Device;
            txtTechnique.Text = recipe.Technique;
            txtKnowledge.Text = recipe.Knowledge;
            txtLevel.Text = recipe.Level.ToString();
            txtTier.Text = recipe.Tier.ToString();
            txtIcon.Text = recipe.Icon.ToString();
            txtPrimaryComp.Text = recipe.PrimaryComp;
            txtBuild1Comp.Text = recipe.Build1Comp;
            txtBuild2Comp.Text = recipe.Build2Comp;
            txtBuild3Comp.Text = recipe.Build3Comp;
            txtBuild4Comp.Text = recipe.Build4Comp;
            txtFuelComp.Text = recipe.FuelComp;
            txtProductName.Text = recipe.Product;
            txtPrimaryQty.Text = recipe.PrimaryQty.ToString();
            txtBuild1Qty.Text = recipe.Build1Qty.ToString();
            txtBuild2Qty.Text = recipe.Build2Qty.ToString();
            txtBuild3Qty.Text = recipe.Build3Qty.ToString();
            txtBuild4Qty.Text = recipe.Build4Qty.ToString();
            txtFuelQty.Text = recipe.FuelQty.ToString();
            txtProductQty.Text = recipe.ProductQty.ToString();

            txtStage0Product.Text = recipe.Stage0Product;
            txtStage0Qty.Text = recipe.Stage0Qty.ToString();
            txtStage0Byproduct.Text = recipe.Stage0ByProduct;
            txtStage0ByQty.Text = recipe.Stage0ByQty.ToString();

            txtStage1Product.Text = recipe.Stage1Product;
            txtStage1Qty.Text = recipe.Stage1Qty.ToString();
            txtStage1Byproduct.Text = recipe.Stage1ByProduct;
            txtStage1ByQty.Text = recipe.Stage1ByQty.ToString();

            txtStage2Product.Text = recipe.Stage2Product;
            txtStage2Qty.Text = recipe.Stage2Qty.ToString();
            txtStage2Byproduct.Text = recipe.Stage2ByProduct;
            txtStage2ByQty.Text = recipe.Stage2ByQty.ToString();

            txtStage3Product.Text = recipe.Stage3Product;
            txtStage3Qty.Text = recipe.Stage3Qty.ToString();
            txtStage3Byproduct.Text = recipe.Stage3ByProduct;
            txtStage3ByQty.Text = recipe.Stage3ByQty.ToString();

            txtStage4Product.Text = recipe.Stage4Product;
            txtStage4Qty.Text = recipe.Stage4Qty.ToString();
            txtStage4Byproduct.Text = recipe.Stage4ByProduct;
            txtStage4ByQty.Text = recipe.Stage4ByQty.ToString();
        }

        public void AddRecipe(Recipe recipe)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate() { AddRecipe(recipe); }));
                return;
            }

            recipes.Add(recipe);
            lbRecipes.Items.Add(recipe.Name);
        }

        private void ProcessRecipes()
        {
            // Disable the buttons while we are parsing the site
            SetButtonEnabled(btnExport, false);
            SetButtonEnabled(btnClear, false);
            Recipes.ProcessSearchPage(this, txtWeb.Text);
            // Enable the buttons now that parsing is done
            SetButtonEnabled(btnExport, true);
            SetButtonEnabled(btnClear, true);
            // Change the label color to green and tell the user how many recipes we parsed
            SetInfoLabel(recipes.Count + " recipe's processed.", Color.Green);
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            if (txtWeb.Text != "")
            {
                Thread thread_recipes = new Thread(new ThreadStart(ProcessRecipes));
                thread_recipes.Start();
            }
        }

        private void lbRecipes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbRecipes.SelectedIndex < 0)
                return;

            DisplayRecipe(recipes[lbRecipes.SelectedIndex]);
        }

        public void SetInfoLabel(string txt, Color color)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate() { SetInfoLabel(txt, color); }));
                return;
            }

            lblInfo.ForeColor = color;
            lblInfo.Text = txt;
        }

        public void SetButtonEnabled (Button btn, bool enabled)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate() { SetButtonEnabled(btn, enabled); }));
                return;
            }

            btn.Enabled = enabled;
        }

        public void SetExportLabel(string txt, Color color)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate() { SetExportLabel(txt, color); }));
                return;
            }

            lblExport.Text = txt;
            lblExport.ForeColor = color;
        }

        private void ExportRecipes()
        {
            // Disable the buttons so the user can not click on them while the export is running
            SetButtonEnabled(btnExport, false);
            SetButtonEnabled(btnClear, false);

            if (frmDatabase.ShowDialog() != DialogResult.OK)
            {
                SetButtonEnabled(btnExport, true);
                SetButtonEnabled(btnClear, true);
                return;
            }

            Database db = new Database();
            if (!db.Connect(frmDatabase.GetHost(), frmDatabase.GetUsername(), frmDatabase.GetPassword(), frmDatabase.GetDatabase(), frmDatabase.GetPort()))
            {
                SetButtonEnabled(btnExport, true);
                SetButtonEnabled(btnClear, true);
                return;
            }


            uint id = db.GetMaxId("recipes", "recipe_id") + 1;
            int count = 1;
            int max = recipes.Count;
            foreach (Recipe r in recipes)
            {
                SetExportLabel("Exporting recipe " + count + " of " + max + "...", Color.Red);

                #region recipes table
                uint technique = db.SingleSelect("SELECT `id` FROM `skills` WHERE `name` = \"{0}\"", Database.Escape(r.Technique));
                uint knowledge = db.SingleSelect("SELECT `id` FROM `skills` WHERE `name` = \"{0}\"", Database.Escape(r.Knowledge));
                uint product_id = db.SingleSelect("SELECT `id` FROM `items` WHERE `name` = \"{0}\"", Database.Escape(r.Product));
                uint product_classes = 0;
                uint deviceID = GetDeviceID(r.Device);
                db.Query("insert into `recipes` (`recipe_id`, `tier`, `level`, `icon`, `skill_level`, `technique`, `knowledge`, `name`, `book`, `device`, `product_classes`, `unknown2`, `product_item_id`, `product_name`, `product_qty`, `primary_comp_title`, `build_comp_title`, `build2_comp_title`, `build3_comp_title`, `build4_comp_title`, `build_comp_qty`, `build2_comp_qty`, `build3_comp_qty`, `build4_comp_qty`, `fuel_comp_title`, `fuel_comp_qty`) VALUES ({0}, {1}, {2}, {3}, {4}, {5}, {6}, '{7}', '{8}', '{9}', {10}, {11}, {12}, '{13}', {14}, '{15}', '{16}', '{17}', '{18}', '{19}', {20}, {21}, {22}, {23}, '{24}', {25})",
                    id, GetTierId(r.Tier), r.Level, r.Icon, 0, technique, knowledge, Database.Escape(r.Name), Database.Escape(r.Book), Database.Escape(r.Device), product_classes, deviceID, product_id, Database.Escape(r.Product), r.ProductQty, Database.Escape(r.PrimaryComp), Database.Escape(r.Build1Comp), Database.Escape(r.Build2Comp), Database.Escape(r.Build3Comp), Database.Escape(r.Build4Comp), r.Build1Qty, r.Build2Qty, r.Build3Qty, r.Build4Qty, Database.Escape(r.FuelComp), r.FuelQty);
                #endregion

                #region recipe_components table
                // Every recipe will have a primary component
                uint item_id = db.SingleSelect("SELECT `id` FROM `items` WHERE `name` = \"{0}\"", Database.Escape(GetComponentName(r.PrimaryComp)));
                db.Query("insert into `recipe_components` (`recipe_id`, `item_id`, `slot_id`) VALUES ({0}, {1}, {2})",
                    id, item_id, 0);
                // Need to check the build components as not all of them will be used
                if (r.Build1Comp != null && r.Build1Comp != "")
                {
                    item_id = db.SingleSelect("SELECT `id` FROM `items` WHERE `name` = \"{0}\"", Database.Escape(GetComponentName(r.Build1Comp)));
                    db.Query("insert into `recipe_components` (`recipe_id`, `item_id`, `slot_id`) VALUES ({0}, {1}, {2})",
                        id, item_id, 1);
                }
                if (r.Build2Comp != null && r.Build2Comp != "")
                {
                    item_id = db.SingleSelect("SELECT `id` FROM `items` WHERE `name` = \"{0}\"", Database.Escape(GetComponentName(r.Build2Comp)));
                    db.Query("insert into `recipe_components` (`recipe_id`, `item_id`, `slot_id`) VALUES ({0}, {1}, {2})",
                        id, item_id, 2);
                }
                if (r.Build3Comp != null && r.Build3Comp != "")
                {
                    item_id = db.SingleSelect("SELECT `id` FROM `items` WHERE `name` = \"{0}\"", Database.Escape(GetComponentName(r.Build3Comp)));
                    db.Query("insert into `recipe_components` (`recipe_id`, `item_id`, `slot_id`) VALUES ({0}, {1}, {2})",
                        id, item_id, 3);
                }
                if (r.Build4Comp != null && r.Build4Comp != "")
                {
                    item_id = db.SingleSelect("SELECT `id` FROM `items` WHERE `name` = \"{0}\"", Database.Escape(GetComponentName(r.Build4Comp)));
                    db.Query("insert into `recipe_components` (`recipe_id`, `item_id`, `slot_id`) VALUES ({0}, {1}, {2})",
                        id, item_id, 4);
                }

                // Every recipe will have a fuel component
                item_id = db.SingleSelect("SELECT `id` FROM `items` WHERE `name` = \"{0}\"", Database.Escape(GetComponentName(r.FuelComp)));
                db.Query("insert into `recipe_components` (`recipe_id`, `item_id`, `slot_id`) VALUES ({0}, {1}, {2})",
                    id, item_id, 5);
                #endregion

                #region recipe_products table
                item_id = db.SingleSelect("SELECT `id` FROM `items` WHERE `name` = \"{0}\"", Database.Escape(r.Stage0Product));
                uint item2_id = 0;
                if (r.Stage0ByProduct != null && r.Stage0ByProduct != "")
                    item2_id = db.SingleSelect("SELECT `id` FROM `items` WHERE `name` = \"{0}\"", Database.Escape(r.Stage0ByProduct));
                else
                    item2_id = 0;

                db.Query("insert into `recipe_products` (`recipe_id`, `stage`, `product_id`, `byproduct_id`, `product_qty`, `byproduct_qty`) VALUES ({0}, {1}, {2}, {3}, {4}, {5})",
                    id, 0, item_id, item2_id, r.Stage0Qty, r.Stage0ByQty);

                item_id = db.SingleSelect("SELECT `id` FROM `items` WHERE `name` = \"{0}\"", Database.Escape(r.Stage1Product));
                if (r.Stage1ByProduct != null && r.Stage1ByProduct != "")
                    item2_id = db.SingleSelect("SELECT `id` FROM `items` WHERE `name` = \"{0}\"", Database.Escape(r.Stage1ByProduct));
                else
                    item2_id = 0;
                db.Query("insert into `recipe_products` (`recipe_id`, `stage`, `product_id`, `byproduct_id`, `product_qty`, `byproduct_qty`) VALUES ({0}, {1}, {2}, {3}, {4}, {5})",
                    id, 1, item_id, item2_id, r.Stage1Qty, r.Stage1ByQty);

                item_id = db.SingleSelect("SELECT `id` FROM `items` WHERE `name` = \"{0}\"", Database.Escape(r.Stage2Product));
                if (r.Stage2ByProduct != null && r.Stage2ByProduct != "")
                    item2_id = db.SingleSelect("SELECT `id` FROM `items` WHERE `name` = \"{0}\"", Database.Escape(r.Stage2ByProduct));
                else
                    item2_id = 0;
                db.Query("insert into `recipe_products` (`recipe_id`, `stage`, `product_id`, `byproduct_id`, `product_qty`, `byproduct_qty`) VALUES ({0}, {1}, {2}, {3}, {4}, {5})",
                    id, 2, item_id, item2_id, r.Stage2Qty, r.Stage2ByQty);

                item_id = db.SingleSelect("SELECT `id` FROM `items` WHERE `name` = \"{0}\"", Database.Escape(r.Stage3Product));
                if (r.Stage3ByProduct != null && r.Stage3ByProduct != "")
                    item2_id = db.SingleSelect("SELECT `id` FROM `items` WHERE `name` = \"{0}\"", Database.Escape(r.Stage3ByProduct));
                else
                    item2_id = 0;
                db.Query("insert into `recipe_products` (`recipe_id`, `stage`, `product_id`, `byproduct_id`, `product_qty`, `byproduct_qty`) VALUES ({0}, {1}, {2}, {3}, {4}, {5})",
                    id, 3, item_id, item2_id, r.Stage3Qty, r.Stage3ByQty);

                item_id = db.SingleSelect("SELECT `id` FROM `items` WHERE `name` = \"{0}\"", Database.Escape(r.Stage4Product));
                if (r.Stage4ByProduct != null && r.Stage4ByProduct != "")
                    item2_id = db.SingleSelect("SELECT `id` FROM `items` WHERE `name` = \"{0}\"", Database.Escape(r.Stage4ByProduct));
                else
                    item2_id = 0;
                db.Query("insert into `recipe_products` (`recipe_id`, `stage`, `product_id`, `byproduct_id`, `product_qty`, `byproduct_qty`) VALUES ({0}, {1}, {2}, {3}, {4}, {5})",
                    id, 4, item_id, item2_id, r.Stage4Qty, r.Stage4ByQty);
                #endregion

                id++;
                count++;
            }

            // Close the MySQL connection
            db.Close();
            // Enable the buttons now that the export is complete
            SetButtonEnabled(btnExport, true);
            SetButtonEnabled(btnClear, true);
            SetExportLabel("Exporting complete.", Color.Green);
        }

        private uint GetDeviceID(string device)
        {
            uint ret = 0;
            switch (device)
            {
                case "Sewing Table & Mannequin":
                    ret = 1;
                    break;
                case "Forge":
                    ret = 2;
                    break;
                case "Chemistry Table":
                    ret = 3;
                    break;
                case "Engraved Desk":
                    ret = 4;
                    break;
                case "Work Bench":
                    ret = 5;
                    break;
                case "Woodworking Table":
                    ret = 6;
                    break;
                case "Stove & Keg":
                    ret = 7;
                    break;
            }

            return ret;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            // Return out if we have no data to export to the DB
            if (recipes.Count == 0)
                return;

            Thread thread_recipes = new Thread(new ThreadStart(ExportRecipes));
            thread_recipes.Start();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

            txtName.Text = null;
            txtBook.Text = null;
            txtDevice.Text = null;
            txtTechnique.Text = null;
            txtKnowledge.Text = null;
            txtLevel.Text = null;
            txtTier.Text = null;
            txtIcon.Text = null;
            txtPrimaryComp.Text = null;
            txtBuild1Comp.Text = null;
            txtBuild2Comp.Text = null;
            txtBuild3Comp.Text = null;
            txtBuild4Comp.Text = null;
            txtFuelComp.Text = null;
            txtProductName.Text = null;
            txtPrimaryQty.Text = null;
            txtBuild1Qty.Text = null;
            txtBuild2Qty.Text = null;
            txtBuild3Qty.Text = null;
            txtBuild4Qty.Text = null;
            txtFuelQty.Text = null;
            txtProductQty.Text = null;

            txtStage0Product.Text = null;
            txtStage0Qty.Text = null;
            txtStage0Byproduct.Text = null;
            txtStage0ByQty.Text = null;

            txtStage1Product.Text = null;
            txtStage1Qty.Text = null;
            txtStage1Byproduct.Text = null;
            txtStage1ByQty.Text = null;

            txtStage2Product.Text = null;
            txtStage2Qty.Text = null;
            txtStage2Byproduct.Text = null;
            txtStage2ByQty.Text = null;

            txtStage3Product.Text = null;
            txtStage3Qty.Text = null;
            txtStage3Byproduct.Text = null;
            txtStage3ByQty.Text = null;

            txtStage4Product.Text = null;
            txtStage4Qty.Text = null;
            txtStage4Byproduct.Text = null;
            txtStage4ByQty.Text = null;

            recipes.Clear();
            lbRecipes.Items.Clear();
            lblInfo.Text = null;
        }

        private string GetComponentName(string componentTitle)
        {
            string ret = null;

            switch (componentTitle)
            {
                case "Raw Tin": //tier 1
                    ret = "tin cluster";
                    break;
                case "Raw Bronze":
                    ret = "bronze cluster";
                    break;
                case "Raw Malachite":
                    ret = "rough malachite";
                    break;
                case "Raw Lapis Lazuli":
                    ret = "Raw Lapis Lazuli";
                    break;
                case "Raw Rawhide Leather":
                    ret = "rawhide leather pelt";
                    break;
                case "Raw Waxed Leather":
                    ret = "waxed leather pelt";
                    break;
                case "Raw Root":
                    ret = "root";
                    break;
                case "Raw Roots":
                    ret = "root";
                    break;
                case "Raw Yarrow":
                    ret = "yarrow";
                    break;
                case "Raw Lead":
                    ret = "lead cluster";
                    break;
                case "Raw Copper":
                    ret = "copper cluster";
                    break;
                case "Leaded Loam":
                    ret = "leaded loam";
                    break;
                case "Solidified Loam":
                    ret = "solidified loam";
                    break;
                case "Raw Elm":
                    ret = "severed elm";
                    break;
                case "Raw Alder":
                    ret = "severed alder";
                    break;
                case "Raw Iron": //tier 2
                    ret = "iron cluster";
                    break;
                case "Salty Loam":
                    ret = "salty loam";
                    break;
                case "Raw Maple":
                    ret = "severed maple";
                    break;
                case "Raw Blackened Iron":
                    ret = "blackened iron cluster";
                    break;
                case "Alkaline Loam":
                    ret = "alkaline loam";
                    break;
                case "Raw Bone":
                    ret = "severed bone";
                    break;
                case "Raw Tuber Strand":
                    ret = "tuber strand";
                    break;
                case "Raw Tuber Strands":
                    ret = "tuber strand";
                    break;
                case "Raw Sisal":
                    ret = "sisal root";
                    break;
                case "Raw Antonican Coffee Bean":
                    ret = "Antonican coffee bean";
                    break;
                case "Raw Black Tea Leaf":
                    ret = "black tea leaf";
                    break;
                case "Raw Orange":
                    ret = "murdunk orange";
                    break;
                case "Raw Carrot":
                    ret = "raw carrot";
                    break;
                case "Raw Vulrich Meat":
                    ret = "vulrich meat";
                    break;
                case "Raw Elephant meat":
                    ret = "elephant meat";
                    break;
                case "Raw Tanned Leather":
                    ret = "tanned leather pelt";
                    break;
                case "Raw Cured Leather":
                    ret = "cured leather pelt";
                    break;
                case "Raw Crab Meat":
                    ret = "crab meat";
                    break;
                case "Raw Grouper":
                    ret = "freewater grouper";
                    break;
                case "Raw Electrum":
                    ret = "electrum cluster";
                    break;
                case "Raw Turquoise":
                    ret = "rough turquoise";
                    break;
                case "Raw Silver":
                    ret = "silver cluster";
                    break;
                case "Raw Coral":
                    ret = "rough coral";
                    break;
                case "Raw Lion Meat": //tier 3
                    ret = "lion meat";
                    break;
                case "Raw Pig Meat":
                    ret = "Pig Meat";
                    break;
                case "Raw Boiled Leather":
                    ret = "boiled leather pelt";
                    break;
                case "Raw Cuirboilli Leather":
                    ret = "cuirboilli leather pelt";
                    break;
                case "Raw Mackerel":
                    ret = "seafury mackerel";
                    break;
                case "Raw Crayfish":
                    ret = "thicket crayfish";
                    break;
                case "Raw Carbonite":
                    ret = "carbonite cluster";
                    break;
                case "Pliant Loam":
                    ret = "pliant loam";
                    break;
                case "Raw Steel":
                    ret = "steel cluster";
                    break;
                case "Malleable Loam":
                    ret = "malleable loam";
                    break;
                case "Raw Belladonna Root":
                    ret = "belladonna root";
                    break;
                case "Raw Dandelion":
                    ret = "dandelion fiber";
                    break;
                case "Raw Steppes Mountain Bean":
                    ret = "Steppes mountain bean";
                    break;
                case "Raw Oolong Tea Leaf":
                    ret = "oolong tea leaf";
                    break;
                case "Raw Fayberry":
                    ret = "fayberry";
                    break;
                case "Raw Sweet Onion":
                    ret = "sweet onion";
                    break;
                case "Raw Gold":
                    ret = "gold cluster";
                    break;
                case "Raw Agate":
                    ret = "rough agate";
                    break;
                case "Raw Palladium":
                    ret = "palladium cluster";
                    break;
                case "Raw Jasper":
                    ret = "rough jasper";
                    break;
                case "Raw Ash":
                    ret = "severed ash";
                    break;
                case "Raw Fir":
                    ret = "severed fir";
                    break;
                case "Raw Bear Meat": //tier 4
                    ret = "bear meat";
                    break;
                case "Raw Griffon Meat":
                    ret = "griffon meat";
                    break;
                case "Raw Etched Leather":
                    ret = "etched leather pelt";
                    break;
                case "Raw Engraved Leather":
                    ret = "engraved leather pelt";
                    break;
                case "Raw Carp":
                    ret = "murkwater carp";
                    break;
                case "Raw Shark Fin":
                    ret = "shark fin";
                    break;
                case "Raw Feyiron":
                    ret = "feyiron cluster";
                    break;
                case "Supple Loam":
                    ret = "supple loam";
                    break;
                case "Raw Feysteel":
                    ret = "feysteel cluster";
                    break;
                case "Ductile Loam":
                    ret = "ductile loam";
                    break;
                case "Raw Tussah Root":
                    ret = "tussah root";
                    break;
                case "Raw Oak Root":
                    ret = "oak root";
                    break;
                case "Raw Oak Roots":
                    ret = "oak roots";
                    break;
                case "Raw Tussah Roots":
                    ret = "tussah root";
                    break;
                case "Raw Everfrost Ice Bean":
                    ret = "Everfrost ice bean";
                    break;
                case "Raw Green Tea Leaf":
                    ret = "green tea leaf";
                    break;
                case "Wild Apple":
                    ret = "wild apple";
                    break;
                case "Raw Cucumber":
                    ret = "cucumber";
                    break;
                case "Raw Velium":
                    ret = "velium cluster";
                    break;
                case "Raw Opaline":
                    ret = "rough opaline";
                    break;
                case "Raw Ruthenium":
                    ret = "ruthenium cluster";
                    break;
                case "Raw Opal":
                    ret = "rough opal";
                    break;
                case "Raw Briarwood":
                    ret = "severed briarwood";
                    break;
                case "Raw Oak":
                    ret = "severed oak";
                    break;
                case "Raw Owlbear Meat": // tier 5
                    ret = "owlbeat meat";
                    break;
                case "Raw Manticore Meat":
                    ret = "manticore meat";
                    break;
                case "Raw Wyrm Meat":
                    ret = "wyrm meat";
                    break;
                case "Raw Strengthened Leather":
                    ret = "strengthened leather pelt";
                    break;
                case "Raw Augmented Leather":
                    ret = "augmented leather pelt";
                    break;
                case "Raw Blowfish":
                    ret = "cauldron blowfish";
                    break;
                case "Raw Trout":
                    ret = "nerius trout";
                    break;
                case "Raw Fulginate":
                    ret = "fulginate cluster";
                    break;
                case "Raw Ebon":
                    ret = "ebon cluster";
                    break;
                case "Raw Ashen Root":
                    ret = "ashen root";
                    break;
                case "Raw Figwart Roots":
                    ret = "figwart root";
                    break;
                case "Raw Ashen Roots":
                    ret = "ashen roots";
                    break;
                case "Raw Lavastorm Robusta Bean":
                    ret = "Lavastorm robusta bean";
                    break;
                case "Raw Pu-erh Tea Leaf":
                    ret = "pu-erh tea leaf";
                    break;
                case "Raw White Peach":
                    ret = "white peach";
                    break;
                case "Raw Browncap Mushroom":
                    ret = "browncap mushroom";
                    break;
                case "Raw Diamondine":
                    ret = "diamondine cluster";
                    break;
                case "Raw Bloodstone":
                    ret = "rough bloodstone";
                    break;
                case "Raw Rhodium":
                    ret = "rhodium cluster";
                    break;
                case "Raw Ruby":
                    ret = "rough ruby";
                    break;
                case "Raw Teak":
                    ret = "severed teak";
                    break;
                case "Raw Cedar":
                    ret = "severed cedar";
                    break;
                case "Raw Sabertooth Meat": //tier 6
                    ret = "sabertooth meat";
                    break;
                case "Raw Caiman Meat":
                    ret = "caiman meat";
                    break;
                case "Raw Stonehide Leather":
                    ret = "stonehide leather pelt";
                    break;
                case "Raw Scaled Leather":
                    ret = "scaled leather pelt";
                    break;
                case "Raw Eel":
                    ret = "conger eel";
                    break;
                case "Raw Tiger Shrimp":
                    ret = "tiger shrimp";
                    break;
                case "Raw Tigershrimp":
                    ret = "tiger shrimp";
                    break;
                case "Raw Indium":
                    ret = "indium cluster";
                    break;
                case "Raw Cobalt":
                    ret = "cobalt cluster";
                    break;
                case "Raw Succulent Root":
                    ret = "succulent root";
                    break;
                case "Raw Succulent Roots":
                    ret = "succulent root";
                    break;
                case "Raw Saguaro Root":
                    ret = "saguaro root";
                    break;
                case "Raw Saguaro Roots":
                    ret = "saguaro root";
                    break;
                case "Raw Maj'Dul Coffee Bean":
                    ret = "Maj'Dul coffee bean";
                    break;
                case "Raw Darjeeling Tea Leaf":
                    ret = "darjeeling tea leaf";
                    break;
                case "Raw Prickly Pear":
                    ret = "prickly pear";
                    break;
                case "Raw Berryllium":
                    ret = "berryllium cluster";
                    break;
                case "Raw Nacre":
                    ret = "rough nacre";
                    break;
                case "Raw Vanadium":
                    ret = "vanadium cluster";
                    break;
                case "Raw Pearl":
                    ret = "rough pearl";
                    break;
                case "Raw Sandalwood":
                    ret = "severed sandalwood";
                    break;
                case "Raw Ironwood":
                    ret = "severed ironwood";
                    break;
                case "Raw Ravasect Meat": // tier 7
                    ret = "ravasect meat";
                    break;
                case "Raw Aviak Meat":
                    ret = "aviak meat";
                    break;
                case "Raw Horned Leather":
                    ret = "horned leather pelt";
                    break;
                case "Raw Dragonhide Leather":
                    ret = "dragonhide leather pelt";
                    break;
                case "Raw Flying Fish":
                    ret = "flying fish";
                    break;
                case "Raw Adamantite":
                    ret = "adamantite cluster";
                    break;
                case "Raw Xegonite":
                    ret = "xegonite cluster";
                    break;
                case "Raw Hanging Root":
                    ret = "hanging root";
                    break;
                case "Raw Hanging Roots":
                    ret = "hanging root";
                    break;
                case "Raw Nimbus Root":
                    ret = "nimbus root";
                    break;
                case "Raw Nimbus Roots":
                    ret = "nimbus root";
                    break;
                case "Raw Azurite":
                    ret = "azurite cluster";
                    break;
                case "Raw Topaz":
                    ret = "rough topaz";
                    break;
                case "Raw Acrylia":
                    ret = "acrylia cluster";
                    break;
                case "Raw Moonstone":
                    ret = "rough moonstone";
                    break;
                case "Raw Rosewood":
                    ret = "rough lumbered rosewood";
                    break;
                case "Raw Ebony":
                    ret = "rough lumbered ebony";
                    break;
                case "Bristled Leather": // tier 8
                    ret = "bristled pelt";
                    break;
                case "Hidebound Leather":
                    ret = "hidebound pelt";
                    break;
                case "Raw Barracuda":
                    ret = "barracuda";
                    break;
                case "Magma Fish":
                    ret = "magma rock fish";
                    break;
                case "Raw Mantrap Root":
                    ret = "mantrap root";
                    break;
                case "Cranberry":
                    ret = "raw cranberry";
                    break;
                case "Raw Succulent":
                    ret = "raw succulent petal";
                    break;
                case "Succulent":
                    ret = "raw succulent petal";
                    break;
                case "Raw Kunzite":
                    ret = "rough kunzite";
                    break;
                case "Mottled Leather": // tier 9
                    ret = "mottled pelt";
                    break;
                case "Spotted Leather":
                    ret = "spotted pelt";
                    break;
                case "Bamboo Shoots":
                    ret = "bamboo shoot";
                    break;
                case "Toxnettle Roots":
                    ret = "toxnettle root";
                    break;
                case "Thick Bear Leather": //tier 10
                    ret = "thick bear pelt";
                    break;
                case "Metallic Reptile Leather":
                    ret = "metallic reptile hide";
                    break;
                case "Liquid": //cooking components
                    ret = "aerated mineral water";
                    break;
            }
            // If ret = null return the string that was passed to this function
            return ret == null ? componentTitle : ret;
        }

        private byte GetTierId(string tierName)
        {
            byte ret = 0;

            switch (tierName)
            {
                case "Handcrafted":
                    ret = 1;
                    break;
                case "Mastercrafted":
                    ret = 5;
                    break;
                case "Mastercrafted legendary":
                    ret = 7;
                    break;
                case "Mastercrafted fabled":
                    ret = 9;
                    break;
                case "Mastercrafted mythical":
                    ret = 9;
                    break;
            }

            return ret;
        }
    }
}
