using System;
using System.Collections;
using System.Collections.Generic;
using MvvmCross.DroidX.RecyclerView;

namespace MvvmCross.AdvancedRecyclerView.Adapters
{
    public interface IMvxAdvancedRecyclerViewAdapter
    {
        IEnumerable ItemsSource { get; set; }
    }

}
