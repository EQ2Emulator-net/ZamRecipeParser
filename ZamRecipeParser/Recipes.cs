using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using HtmlAgilityPack;

namespace ZamRecipeParser
{
    public class Recipe
    {
        public string Name;
        public string Book;
        public string Device;
        public string Technique;
        public string Knowledge;
        public byte Level;
        public string Tier;
        public UInt16 Icon;

        public string PrimaryComp;
        public string Build1Comp;
        public string Build2Comp;
        public string Build3Comp;
        public string Build4Comp;
        public string FuelComp;
        public string Product;

        public byte PrimaryQty;
        public byte Build1Qty;
        public byte Build2Qty;
        public byte Build3Qty;
        public byte Build4Qty;
        public byte FuelQty;
        public int ProductQty;

        public string Stage0Product;
        public int Stage0Qty;
        public string Stage0ByProduct;
        public byte Stage0ByQty;

        public string Stage1Product;
        public int Stage1Qty;
        public string Stage1ByProduct;
        public byte Stage1ByQty;

        public string Stage2Product;
        public int Stage2Qty;
        public string Stage2ByProduct;
        public byte Stage2ByQty;

        public string Stage3Product;
        public int Stage3Qty;
        public string Stage3ByProduct;
        public byte Stage3ByQty;

        public string Stage4Product;
        public int Stage4Qty;
        public string Stage4ByProduct;
        public byte Stage4ByQty;

        public Recipe()
        {
            Name = null;
            Book = null;
            Device = null;
            Technique = null;
            Knowledge = null;
            Level = 0;
            Tier = null;
            Icon = 0;
            PrimaryComp = null;
            Build1Comp = null;
            Build2Comp = null;
            Build3Comp = null;
            Build4Comp = null;
            FuelComp = null;
            Product = null;
            PrimaryQty = 0;
            Build1Qty = 0;
            Build2Qty = 0;
            Build3Qty = 0;
            Build4Qty = 0;
            FuelQty = 0;
            ProductQty = 0;

            Stage0Product = null;
            Stage0Qty = 0;
            Stage0ByProduct = null;
            Stage0ByQty = 0;

            Stage1Product = null;
            Stage1Qty = 0;
            Stage1ByProduct = null;
            Stage1ByQty = 0;

            Stage2Product = null;
            Stage2Qty = 0;
            Stage2ByProduct = null;
            Stage2ByQty = 0;

            Stage3Product = null;
            Stage3Qty = 0;
            Stage3ByProduct = null;
            Stage3ByQty = 0;

            Stage4Product = null;
            Stage4Qty = 0;
            Stage4ByProduct = null;
            Stage4ByQty = 0;
        }
    }

    public class Recipes
    {
        private static void AddComponent(Recipe recipe, string slot, string component, byte quantity)
        {
            if (component != null)
                component = component.Trim();

            switch (slot)
            {
                case "Primary:":
                    recipe.PrimaryComp = component;
                    recipe.PrimaryQty = quantity;
                    break;
                case "Build 1:":
                    recipe.Build1Comp = component;
                    recipe.Build1Qty = quantity;
                    break;
                case "Build 2:":
                    recipe.Build2Comp = component;
                    recipe.Build2Qty = quantity;
                    break;
                case "Build 3:":
                    recipe.Build3Comp = component;
                    recipe.Build3Qty = quantity;
                    break;
                case "Build 4:":
                    recipe.Build4Comp = component;
                    recipe.Build4Qty = quantity;
                    break;
                case "Fuel:":
                    recipe.FuelComp = component;
                    recipe.FuelQty = quantity;
                    break;
            }
        }

        private static void SetResults(Recipe recipe, string stage, string product, int qty, string byProduct, byte byQty)
        {
            if (product != null)
                product = product.Trim();
            if (byProduct != null)
                byProduct = byProduct.Trim();

            switch (stage)
            {
                case "Pristine:":
                    recipe.Stage4Product = product;
                    recipe.Stage4ByProduct = byProduct;
                    recipe.Stage4Qty = qty;
                    recipe.Stage4ByQty = byQty;

                    recipe.Product = product;
                    recipe.ProductQty = qty;
                    break;
                case "Third Bar:":
                    recipe.Stage3Product = product;
                    recipe.Stage3ByProduct = byProduct;
                    recipe.Stage3Qty = qty;
                    recipe.Stage3ByQty = byQty;
                    break;
                case "Second Bar:":
                    recipe.Stage2Product = product;
                    recipe.Stage2ByProduct = byProduct;
                    recipe.Stage2Qty = qty;
                    recipe.Stage2ByQty = byQty;
                    break;
                case "First Bar:":
                    recipe.Stage1Product = product;
                    recipe.Stage1ByProduct = byProduct;
                    recipe.Stage1Qty = qty;
                    recipe.Stage1ByQty = byQty;
                    break;
                case "Failure:":
                    recipe.Stage0Product = product;
                    recipe.Stage0ByProduct = byProduct;
                    recipe.Stage0Qty = qty;
                    recipe.Stage0ByQty = byQty;
                    break;
            }
        }

        private static string GetDeviceName(string device)
        {
            string ret = null;

            switch (device)
            {
                // 'Chemistry Table','Engraved Desk','Forge','Stove & Keg','Sewing Table & Mannequin','Woodworking Table','Work Bench'
                case "Sewing Table":
                    ret = "Sewing Table & Mannequin";
                    break;
                case "Work Desk":
                    ret = "Engraved Desk";
                    break;
                case "Stove And Keg":
                    ret = "Stove & Keg";
                    break;

                // Special devices, just assign a generic one that best matches for now
                case "Sathirian Alch Table":
                case "Froglok Bucket":
                case "Snowfang Pot":
                    ret = "Chemistry Table";
                    break;
                case "Brewing Stump":
                case "Exp03 Cauldron":
                case "Stove":
                case "Everfrost Keg":
                    ret = "Stove & Keg";
                    break;
                case "Quest Defiler Loom":
                case "Everfrost Sewingtable":
                case "Nest Loom":
                    ret = "Sewing Table & Mannequin";
                    break;
                case "Crafting Intro Anvil":
                case "Draconite Forge":
                case "Tower Forge":
                case "Sootfoot Forge":
                case "Orc Forge":
                case "Najena Forge":
                case "Bcg Forge":
                case "Drunder Great Forge":
                case "Snowfang Fire":
                case "Prayer Shawl Fire":
                    ret = "Forge";
                    break;
                case "Everfrost Workbench":
                    ret = "Woodworking Table";
                    break;
                case "Tunarian Work Bench":
                case "Hyperspacial Work Bench":
                case "Tower Work Bench":
                case "Spacial Work Bench":
                case "Dinree Workbench":
                case "Nest Workbench":
                case "Tinkered Workstation":
                case "":                        // Only stumbled across 1 recipes with no creation device and it had a duplicate entry with creation device set to work bench so default to that
                case " ":
                    ret = "Work Bench";
                    break;
                case "Tower Desk":
                    ret = "Engraved Desk";
                    break;
            }

            return ret == null ? device : ret;
        }

        private static bool ProcessRecipePage(Form1 form, string page)
        {
            HtmlDocument doc = new HtmlWeb().Load(page);
            List<HtmlNode> tr = doc.DocumentNode.SelectNodes("//table[@class='db-infobox']/tr").ToList();

          
            Recipe recipe = new Recipe();

            #region General info table on the right side
            HtmlNode name = doc.DocumentNode.SelectSingleNode("//ul[@class='path']/li[last()]");
            recipe.Name = name.InnerText;

            // 0 = class, don't need so skip

            // 1 = technique
            HtmlNode td = tr[1].SelectSingleNode("./td");
            recipe.Technique = td.InnerText.Trim();

            // 2 = knowledge
            td = tr[2].SelectSingleNode("./td");
            recipe.Knowledge = td.InnerText.Trim();

            // 3 = level
            td = tr[3].SelectSingleNode("./td");
            recipe.Level = byte.Parse(td.InnerHtml);

            // 4 = tier name
            td = tr[5].SelectSingleNode("./td");
            recipe.Tier = td.InnerHtml.Trim();

            // 5 = level tier, don't need this so skip

            // 6 = device
            td = tr[6].SelectSingleNode("./td");
            recipe.Device = GetDeviceName(td.InnerHtml.Trim());
            #endregion

            #region Components table
            // Get the components and produces tables
            HtmlNode table = doc.DocumentNode.SelectSingleNode("//table[@class='recipes']/tr/td/table[@class='databox']");

            tr = table.SelectNodes("./tr").ToList();
            foreach (HtmlNode node in tr)
            {
                string slot;
                string component;
                byte qty;
                slot = node.SelectSingleNode("./th").InnerText;
                td = node.SelectSingleNode("./td");
                if (td.InnerText.Contains(" x"))
                {
                    string[] split = td.InnerText.Split(new string[] { " x" }, StringSplitOptions.None);
                    int index = split.Count();
                    component = td.InnerText.Substring(0, td.InnerText.Length - (split[index - 1].Length + 2));  // + 2 for the " x"
                    qty = byte.Parse(split[index - 1]);
                }
                else
                {
                    component = td.InnerText;
                    qty = 1;
                }
                AddComponent(recipe, slot, component, qty);
            }
            #endregion

            #region Produces table
            table = doc.DocumentNode.SelectSingleNode("//table[@class='recipes']/tr/td/td/table[@class='databox']");
            tr = table.SelectNodes("./tr").ToList();

            foreach (HtmlNode node in tr)
            {
                string stage = null;
                string product = null;
                string byProduct = null;
                int qty = 0;
                byte byQty = 0;

                stage = node.SelectSingleNode("./th").InnerText;

                td = node.SelectSingleNode("./td");

                // Get the icon from the image used for the pristine stage
                if (stage == "Pristine:")
                {
                    HtmlNode img = td.SelectSingleNode("./a/var/img");
                    // Possible the product has no valid picture so check the url first before we try to pull the icon
                    if (img.Attributes["src"].Value.Contains("icon_item_"))
                    {
                        string[] split2 = img.Attributes["src"].Value.Split(new string[] { "icon_item_" }, StringSplitOptions.None);
                        recipe.Icon = UInt16.Parse(split2[1].Substring(0, split2[1].Length - 4));
                    }
                }

                // Get the product name and qty
                if (td.InnerText.Contains(" x"))
                {
                    // Split the string by " x"
                    string[] split = td.InnerText.Split(new string[] { " x" }, StringSplitOptions.None);
                    // See how many results we have after the split
                    int index = split.Count();
                    // Only the last result is the quantity so take the orginal string and remove the last result and the " x"
                    product = td.InnerText.Substring(0, td.InnerText.Length - (split[index - 1].Length + 2));  // + 2 for the " x"
                    qty = int.Parse(split[index - 1]);
                }
                else
                {
                    product = td.InnerText;
                    qty = 1;
                }

                // Get the By-Product name and qty
                td = node.SelectSingleNode("./td[@class='byprod']");
                // make sure there is a by product
                if (td.InnerText != null && td.InnerText != "")
                {
                    // Get the by-product and by-product qty
                    if (td.InnerText.Contains(" x"))
                    {
                        string[] split = td.InnerText.Split(new string[] { " x" }, StringSplitOptions.None);
                        int index = split.Count();
                        byProduct = td.InnerText.Substring(0, td.InnerText.Length - (split[index - 1].Length + 2));  // + 2 for the " x"
                        byQty = byte.Parse(split[index - 1]);
                    }
                    else
                    {
                        byProduct = td.InnerText;
                        byQty = 1;
                    }
                }

                if (byProduct != null && byProduct.Contains("By-Product"))
                {
                    byProduct = byProduct.Substring(10);
                    if (byProduct.Length == 0)
                        byQty = 0;
                }

                SetResults(recipe, stage, product, qty, byProduct, byQty);
            }

            
            #endregion

            #region Learned From table
            tr = doc.DocumentNode.SelectNodes("//table[@class='datatable']/tr").ToList();
            // the first element is the table titles, no actual info so we grab the first row of actual info
            // also not all recipes have a book listed so we need to check to see if there is at least 1 book (size = 2)
            // finally some have multiple books listed so just grab the first as it seems to be duplicate entries
            if (tr.Count >= 2)
                recipe.Book = tr[1].SelectSingleNode("./td").InnerText;
            #endregion

            form.AddRecipe(recipe);
            return true;
        }

        // Get all the recipes on this search page and process them
        private static void ProcessRecipes(Form1 form, string page)
        {
            HtmlDocument doc = new HtmlWeb().Load(page);
            List<HtmlNode> tr = doc.DocumentNode.SelectNodes("//table[@id='RecipeListSortable']/tbody/tr").ToList();

            for (int i = 0; i < tr.Count; i++)
            {
                /* select the first td tag */
                HtmlNode td = tr.ElementAt(i).SelectSingleNode("./td");

                /* the <a> tag in the first <td> tag contains the recipe script name for the page that has the recipe's details */
                HtmlNode a = td.SelectSingleNode("./a");

                string script_name = a.Attributes["href"].Value;
                string temp_page = "http://eq2.zam.com" + script_name;
                ProcessRecipePage(form, temp_page);
            }
        }

        public static void ProcessSearchPage(Form1 form, string page)
        {
            // Load the page
            HtmlDocument doc = new HtmlWeb().Load(page);

            int page_min = 1, page_max = 0;

            /* get the total number of pages for zones */
            /* we don't need the last button which is the "Next" button */
            HtmlNodeCollection nodeCollection = doc.DocumentNode.SelectNodes("//div[@class='pagesel']/div[@class='pages']/a[position()<last()]");

            if (nodeCollection != null)
            {
                List<HtmlNode> a = nodeCollection.ToList();
                page_max = int.Parse(a.ElementAt(a.Count - 1).InnerHtml);
            }
            else
                page_max = 1;

            while (page_min <= page_max)
            {
                string temp_page;
                if (page.Contains("?"))
                    temp_page = page + "&page=" + page_min;
                else
                    temp_page = page + "?page=" + page_min;

                form.SetInfoLabel("Processing recipe page " + page_min + " of " + page_max + "...", Color.Red);
                ProcessRecipes(form, temp_page);

                page_min++;
            }
        }
    }
}
