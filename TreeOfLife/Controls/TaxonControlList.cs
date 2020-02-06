using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TreeOfLife.Controls
{
    static class TaxonControlList
    {
        static List<ITaxonControl> _List = new List<ITaxonControl>();

        static public event EventHandler<TaxonControlEventArgs> OnRegisterTaxonControl = null;
        static public event EventHandler<TaxonControlEventArgs> OnInitTaxonControlAfterLoad = null;
        static public event EventHandler<TaxonControlEventArgs> OnUnregisterTaxonControl = null;

        //-------------------------------------------------------------------
        public static void RegisterTaxonControl(ITaxonControl _itc)
        {
            if (_List.IndexOf(_itc) != -1) return;
            _List.Add(_itc);
            Console.WriteLine("Register: " + _itc);
            OnRegisterTaxonControl?.Invoke(null, new TaxonControlEventArgs(_itc));
        }

        //-------------------------------------------------------------------
        public static void InitTaxonControlOnLoad(ITaxonControl _itc)
        {
            Console.WriteLine("Init after load: " + _itc);
            OnInitTaxonControlAfterLoad?.Invoke(null, new TaxonControlEventArgs(_itc));
        }

        //-------------------------------------------------------------------
        public static void UnregisterTaxonControl(ITaxonControl _itc)
        {
            OnUnregisterTaxonControl?.Invoke(null, new TaxonControlEventArgs(_itc));
            _List.Remove(_itc);
            Console.WriteLine("Unregister: " + _itc);
        }

        //-------------------------------------------------------------------
        public static T FindTaxonControl<T>(GUI.ControlContainerInterface _container = null) where T : class
        {
            foreach (ITaxonControl itc in _List)
            {
                if (itc.GetType() == typeof(T))
                {
                    if (_container == null || itc.OwnerContainer == _container)
                        return itc as T;
                }
            }
            return null;
        }

        //-------------------------------------------------------------------
        public static ITaxonControl FindTaxonControl(Type type, GUI.ControlContainerInterface _container = null)
        {
            foreach (ITaxonControl itc in _List)
            {
                if (itc.GetType() == type)
                {
                    if (_container == null || itc.OwnerContainer == _container)
                        return itc;
                }
            }
            return null;
        }

        //-------------------------------------------------------------------
        public static List<GUI.FormContainer> GetAllFloating()
        {
            List<GUI.FormContainer> result = new List<GUI.FormContainer>();
            foreach (ITaxonControl itc in _List)
            {
                if (!(itc is TaxonControl)) continue;
                TaxonControl tuc = itc as TaxonControl;
                if (tuc.ParentForm is GUI.FormContainer)
                {
                    GUI.FormContainer tfc = tuc.ParentForm as GUI.FormContainer;
                    if (result.IndexOf(tfc) == -1)
                        result.Add(tfc);
                }
            }
            return result;
        }

        //-------------------------------------------------------------------
        public static void OnSelectTaxon(TaxonTreeNode _selected) 
        {
            foreach (ITaxonControl itc in _List) 
                itc.OnSelectTaxon(_selected); 
        }
        
        //-------------------------------------------------------------------
        public static void OnReselectTaxon(TaxonTreeNode _selected = null)
        {
            if (_selected == null && TaxonUtils.MainGraph != null)
                _selected = TaxonUtils.MainGraph.Selected;
            foreach (ITaxonControl itc in _List) 
                itc.OnReselectTaxon(_selected); 
        }

        //-------------------------------------------------------------------
        public static void OnBelowChanged(TaxonTreeNode _selected)
        {
            foreach (ITaxonControl itc in _List)
                itc.OnBelowChanged(_selected);
        }

        //-------------------------------------------------------------------
        public static void OnRefreshAll() 
        {
            foreach (ITaxonControl itc in _List) 
                itc.OnRefreshAll(); 
        }

        //-------------------------------------------------------------------
        public static void OnRefreshGraph()
        {
            foreach (ITaxonControl itc in _List)
                itc.OnRefreshGraph();
        }

        //-------------------------------------------------------------------
        public static void SetRoot(TaxonTreeNode _root) 
        {
            foreach (ITaxonControl itc in _List) 
                itc.SetRoot(_root); 
        }

        //-------------------------------------------------------------------
        public static void OnViewRectangleChanged(Rectangle R) 
        {
            foreach (ITaxonControl itc in _List) 
                itc.OnViewRectangleChanged(R); 
        }

        //-------------------------------------------------------------------
        public static void OnTaxonChanged(object sender, TaxonTreeNode _taxon) 
        {
            foreach (ITaxonControl itc in _List) 
                itc.OnTaxonChanged(sender, _taxon); 
        }

        //-------------------------------------------------------------------
        public static void OnAvailableImagesChanged()
        {
            foreach (ITaxonControl itc in _List)
                itc.OnAvailableImagesChanged();
        }

        //-------------------------------------------------------------------
        public static void OnOptionChanged()
        {
            foreach (ITaxonControl itc in _List)
                itc.OnOptionChanged();
        }
    }

    public class TaxonControlEventArgs : EventArgs
    {
        public TaxonControlEventArgs(ITaxonControl _itc) { ITC = _itc; }
        public ITaxonControl ITC { get; set; }
    }
}
