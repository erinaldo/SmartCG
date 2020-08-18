using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ModComprobantes
{
    public class CompContaLista : ObservableCollection<CompContaLista>
    {
        /*
        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            CompContaLista t = e.Item as CompContaLista;
            if (t != null)
            // If filter is turned on, filter completed items.
            {
                if (this.cbCompleteFilter.IsChecked == true && t.Complete == true)
                    e.Accepted = false;
                else
                    e.Accepted = true;
            }
        }
         */ 
    }
}
