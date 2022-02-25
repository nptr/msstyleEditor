// *********************************
// Message from Original Author:
//
// 2008 Jose Menendez Poo
// Please give me credit if you use this code. It's all I ask.
// Contact me for more info: menendezpoo@gmail.com
// *********************************
//
// Original project from http://ribbon.codeplex.com/
// Continue to support and maintain by http://officeribbon.codeplex.com/


using System.ComponentModel;
using System.ComponentModel.Design;

namespace System.Windows.Forms
{
    internal abstract class RibbonElementWithItemCollectionDesigner
        : ComponentDesigner
    {
        #region Props

        /// <summary>
        /// Gets a reference to the Ribbon that owns the item
        /// </summary>
        public abstract Ribbon Ribbon { get; }

        /// <summary>
        /// Gets the collection of items hosted by this item
        /// </summary>
        public abstract RibbonItemCollection Collection { get; }

        /// <summary>
        /// Called when verbs must be retrieved
        /// </summary>
        /// <returns></returns>
        protected virtual DesignerVerbCollection OnGetVerbs()
        {
            return new DesignerVerbCollection(new[] {
               new DesignerVerb("Add Button", AddButton),
               new DesignerVerb("Add ButtonList", AddButtonList),
               new DesignerVerb("Add ItemGroup", AddItemGroup),
               new DesignerVerb("Add Separator", AddSeparator),
               new DesignerVerb("Add TextBox", AddTextBox),
               new DesignerVerb("Add ComboBox", AddComboBox),
               new DesignerVerb("Add ColorChooser", AddColorChooser),
               new DesignerVerb("Add DescriptionMenuItem", AddDescriptionMenuItem),
               new DesignerVerb("Add CheckBox", AddCheckBox),
               new DesignerVerb("Add UpDown", AddUpDown),
               new DesignerVerb("Add Label", AddLabel),
               new DesignerVerb("Add Host", AddHost)
            });
        }

        /// <summary>
        /// Overriden. Passes the verbs to the designer
        /// </summary>
        public override DesignerVerbCollection Verbs => OnGetVerbs();

        #endregion

        #region Methods

        /// <summary>
        /// Creates an item of the speciifed type
        /// </summary>
        /// <param name="t"></param>
        private void CreateItem(Type t)
        {
            CreateItem(Ribbon, Collection, t);
        }

        /// <summary>
        /// Creates an item of the specified type and adds it to the specified collection
        /// </summary>
        /// <param name="ribbon"></param>
        /// <param name="collection"></param>
        /// <param name="t"></param>
        protected virtual void CreateItem(Ribbon ribbon, RibbonItemCollection collection, Type t)
        {
            if (GetService(typeof(IDesignerHost)) is IDesignerHost host && collection != null && ribbon != null)
            {
                DesignerTransaction transaction = host.CreateTransaction("AddRibbonItem_" + Component.Site.Name);

                MemberDescriptor member = TypeDescriptor.GetProperties(Component)["Items"];
                RaiseComponentChanging(member);

                RibbonItem item = host.CreateComponent(t) as RibbonItem;

                if (!(item is RibbonSeparator))
                    if (item != null)
                        item.Text = item.Site.Name;

                collection.Add(item);
                ribbon.OnRegionsChanged();

                RaiseComponentChanged(member, null, null);
                transaction.Commit();
            }
        }

        protected virtual void AddButton(object sender, EventArgs e)
        {
            CreateItem(typeof(RibbonButton));
        }

        protected virtual void AddButtonList(object sender, EventArgs e)
        {
            CreateItem(typeof(RibbonButtonList));
        }

        protected virtual void AddItemGroup(object sender, EventArgs e)
        {
            CreateItem(typeof(RibbonItemGroup));
        }

        protected virtual void AddSeparator(object sender, EventArgs e)
        {
            CreateItem(typeof(RibbonSeparator));
        }

        protected virtual void AddTextBox(object sender, EventArgs e)
        {
            CreateItem(typeof(RibbonTextBox));
        }

        protected virtual void AddComboBox(object sender, EventArgs e)
        {
            CreateItem(typeof(RibbonComboBox));
        }

        protected virtual void AddColorChooser(object sender, EventArgs e)
        {
            CreateItem(typeof(RibbonColorChooser));
        }

        protected virtual void AddDescriptionMenuItem(object sender, EventArgs e)
        {
            CreateItem(typeof(RibbonDescriptionMenuItem));
        }
        protected virtual void AddCheckBox(object sender, EventArgs e)
        {
            CreateItem(typeof(RibbonCheckBox));
        }
        protected virtual void AddUpDown(object sender, EventArgs e)
        {
            CreateItem(typeof(RibbonUpDown));
        }
        protected virtual void AddLabel(object sender, EventArgs e)
        {
            CreateItem(typeof(RibbonLabel));
        }
        protected virtual void AddHost(object sender, EventArgs e)
        {
            CreateItem(typeof(RibbonHost));
        }
        #endregion
    }
}
