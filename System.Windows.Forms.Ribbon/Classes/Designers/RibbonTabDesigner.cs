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
using System.Windows.Forms.Design.Behavior;

namespace System.Windows.Forms
{
    public class RibbonTabDesigner
        : ComponentDesigner
    {
        private Adorner _panelAdorner;

        public override DesignerVerbCollection Verbs => new DesignerVerbCollection(new[] {
          new DesignerVerb("Add Panel", AddPanel)
      });

        public RibbonTab Tab => Component as RibbonTab;

        public void AddPanel(object sender, EventArgs e)
        {
            if (GetService(typeof(IDesignerHost)) is IDesignerHost host && Tab != null)
            {


                DesignerTransaction transaction = host.CreateTransaction("AddPanel" + Component.Site.Name);
                MemberDescriptor member = TypeDescriptor.GetProperties(Component)["Panels"];
                RaiseComponentChanging(member);

                if (host.CreateComponent(typeof(RibbonPanel)) is RibbonPanel panel)
                {
                    panel.Text = panel.Site.Name;

                    //Michael Spradlin 07/05/2013 Added Panel Index code so we can tell where a panel is at on the ribbon.
                    panel.Index = Tab.Panels.Count;

                    if (panel.Index == 0)
                    {
                        panel.IsFirstPanel = true;
                    }
                    else
                    {
                        foreach (RibbonPanel pnl in Tab.Panels)
                        {
                            pnl.IsLastPanel = false;
                        }

                        panel.IsLastPanel = true;
                    }

                    Tab.Panels.Add(panel);
                    Tab.Owner.OnRegionsChanged();
                }

                RaiseComponentChanged(member, null, null);
                transaction.Commit();
            }
        }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            _panelAdorner = new Adorner();

            //Kevin Carbis - another point where exception is thrown by the designer when current is null
            if (RibbonDesigner.Current != null)
            {
                BehaviorService bs = RibbonDesigner.Current.GetBehaviorService();

                if (bs == null) return;

                bs.Adorners.AddRange(new[] { _panelAdorner });

                _panelAdorner.Glyphs.Add(new RibbonPanelGlyph(bs, this, Tab));
            }
        }
    }
}
