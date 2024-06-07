using System;
using System.ComponentModel;
using System.ComponentModel.Design;


namespace msstyleEditor.PropView
{
    internal class NotifyingCollectionEditor : CollectionEditor
    {
        public NotifyingCollectionEditor(Type t)
            : base(t)
        {
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            object result = base.EditValue(context, provider, value);

            /* Modifying a collection property will not result in a call to PropertyDescriptor.SetValue().
             * Only value types seem to get this treatment. Maybe INotifyPropertyChanged properties would
             * also get notified, but we have no such properties. 
             * 
             * Solution: Subclass and call SetValue() ourself. This results in assigning the property again
             * even though it was already changed (silently) by the editor. But i am fine with that quirk..
             */
            context.PropertyDescriptor.SetValue(this, result);
            return result;
        }
    }
}
