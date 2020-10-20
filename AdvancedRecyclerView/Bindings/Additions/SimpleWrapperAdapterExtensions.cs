using System;
using Android.Runtime;
using AndroidX.RecyclerView.Widget;

namespace Com.H6ah4i.Android.Widget.Advrecyclerview.Adapter
{
    public partial class SimpleWrapperAdapter : global::AndroidX.RecyclerView.Widget.RecyclerView.Adapter
    {
        static Delegate cb_onBindViewHolder_Landroidx_recyclerview_widget_RecyclerView_ViewHolder_I;
#pragma warning disable 0169
        static Delegate GetOnBindViewHolder_Landroidx_recyclerview_widget_RecyclerView_ViewHolder_IHandler()
        {
            if (cb_onBindViewHolder_Landroidx_recyclerview_widget_RecyclerView_ViewHolder_I == null)
                cb_onBindViewHolder_Landroidx_recyclerview_widget_RecyclerView_ViewHolder_I = JNINativeWrapper.CreateDelegate((Action<IntPtr, IntPtr, IntPtr, int>)n_OnBindViewHolder_Landroidx_recyclerview_widget_RecyclerView_ViewHolder_I);
            return cb_onBindViewHolder_Landroidx_recyclerview_widget_RecyclerView_ViewHolder_I;
        }

        static void n_OnBindViewHolder_Landroidx_recyclerview_widget_RecyclerView_ViewHolder_I(IntPtr jnienv, IntPtr native__this, IntPtr native_p0, int p1)
        {
            global::Com.H6ah4i.Android.Widget.Advrecyclerview.Adapter.SimpleWrapperAdapter __this = global::Java.Lang.Object.GetObject<global::Com.H6ah4i.Android.Widget.Advrecyclerview.Adapter.SimpleWrapperAdapter>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
            global::Java.Lang.Object p0 = global::Java.Lang.Object.GetObject<global::Java.Lang.Object>(native_p0, JniHandleOwnership.DoNotTransfer);
            __this.OnBindViewHolder(p0 as RecyclerView.ViewHolder, p1);
        }
#pragma warning restore 0169

        static IntPtr id_onBindViewHolder_Landroidx_recyclerview_widget_RecyclerView_ViewHolder_I;
        // Metadata.xml XPath method reference: path="/api/package[@name='com.h6ah4i.android.widget.advrecyclerview.adapter']/class[@name='SimpleWrapperAdapter']/method[@name='onBindViewHolder' and count(parameter)=2 and parameter[1][@type='VH'] and parameter[2][@type='int']]"
        [Register("onBindViewHolder", "(Landroidx/recyclerview/widget/RecyclerView$ViewHolder;I)V", "GetOnBindViewHolder_Landroidx_recyclerview_widget_RecyclerView_ViewHolder_IHandler")]
        public override unsafe void OnBindViewHolder(global::AndroidX.RecyclerView.Widget.RecyclerView.ViewHolder p0, int p1)
        {
            if (id_onBindViewHolder_Landroidx_recyclerview_widget_RecyclerView_ViewHolder_I == IntPtr.Zero)
                id_onBindViewHolder_Landroidx_recyclerview_widget_RecyclerView_ViewHolder_I = JNIEnv.GetMethodID(class_ref, "onBindViewHolder", "(Landroidx/recyclerview/widget/RecyclerView$ViewHolder;I)V");
            IntPtr native_p0 = JNIEnv.ToLocalJniHandle(p0);
            try
            {
                JValue* __args = stackalloc JValue[2];
                __args[0] = new JValue(native_p0);
                __args[1] = new JValue(p1);

                if (GetType() == ThresholdType)
                    JNIEnv.CallVoidMethod(((global::Java.Lang.Object)this).Handle, id_onBindViewHolder_Landroidx_recyclerview_widget_RecyclerView_ViewHolder_I, __args);
                else
                    JNIEnv.CallNonvirtualVoidMethod(((global::Java.Lang.Object)this).Handle, ThresholdClass, JNIEnv.GetMethodID(ThresholdClass, "onBindViewHolder", "(Landroidx/recyclerview/widget/RecyclerView$ViewHolder;I)V"), __args);
            }
            finally
            {
                JNIEnv.DeleteLocalRef(native_p0);
            }
        }



        static Delegate cb_onCreateViewHolder_Landroid_view_ViewGroup_I;
#pragma warning disable 0169
        static Delegate GetOnCreateViewHolder_Landroid_view_ViewGroup_IHandler()
        {
            if (cb_onCreateViewHolder_Landroid_view_ViewGroup_I == null)
                cb_onCreateViewHolder_Landroid_view_ViewGroup_I = JNINativeWrapper.CreateDelegate((Func<IntPtr, IntPtr, IntPtr, int, IntPtr>)n_OnCreateViewHolder_Landroid_view_ViewGroup_I);
            return cb_onCreateViewHolder_Landroid_view_ViewGroup_I;
        }

        static IntPtr n_OnCreateViewHolder_Landroid_view_ViewGroup_I(IntPtr jnienv, IntPtr native__this, IntPtr native_p0, int p1)
        {
            global::Com.H6ah4i.Android.Widget.Advrecyclerview.Adapter.SimpleWrapperAdapter __this = global::Java.Lang.Object.GetObject<global::Com.H6ah4i.Android.Widget.Advrecyclerview.Adapter.SimpleWrapperAdapter>(jnienv, native__this, JniHandleOwnership.DoNotTransfer);
            global::Android.Views.ViewGroup p0 = global::Java.Lang.Object.GetObject<global::Android.Views.ViewGroup>(native_p0, JniHandleOwnership.DoNotTransfer);
            IntPtr __ret = JNIEnv.ToLocalJniHandle(__this.OnCreateViewHolder(p0, p1));
            return __ret;
        }
#pragma warning restore 0169

        static IntPtr id_onCreateViewHolder_Landroid_view_ViewGroup_I;
        // Metadata.xml XPath method reference: path="/api/package[@name='com.h6ah4i.android.widget.advrecyclerview.adapter']/class[@name='SimpleWrapperAdapter']/method[@name='onCreateViewHolder' and count(parameter)=2 and parameter[1][@type='android.view.ViewGroup'] and parameter[2][@type='int']]"
        [Register("onCreateViewHolder", "(Landroid/view/ViewGroup;I)Landroidx/recyclerview/widget/RecyclerView$ViewHolder;", "GetOnCreateViewHolder_Landroid_view_ViewGroup_IHandler")]
        public override unsafe global::AndroidX.RecyclerView.Widget.RecyclerView.ViewHolder OnCreateViewHolder(global::Android.Views.ViewGroup p0, int p1)
        {
            if (id_onCreateViewHolder_Landroid_view_ViewGroup_I == IntPtr.Zero)
                id_onCreateViewHolder_Landroid_view_ViewGroup_I = JNIEnv.GetMethodID(class_ref, "onCreateViewHolder", "(Landroid/view/ViewGroup;I)Landroidx/recyclerview/widget/RecyclerView$ViewHolder;");
            try
            {
                JValue* __args = stackalloc JValue[2];
                __args[0] = new JValue(p0);
                __args[1] = new JValue(p1);

                global::AndroidX.RecyclerView.Widget.RecyclerView.ViewHolder __ret;
                if (GetType() == ThresholdType)
                    __ret = global::Java.Lang.Object.GetObject<global::Java.Lang.Object>(JNIEnv.CallObjectMethod(((global::Java.Lang.Object)this).Handle, id_onCreateViewHolder_Landroid_view_ViewGroup_I, __args), JniHandleOwnership.TransferLocalRef) as RecyclerView.ViewHolder;
                else
                    __ret = global::Java.Lang.Object.GetObject<global::Java.Lang.Object>(JNIEnv.CallNonvirtualObjectMethod(((global::Java.Lang.Object)this).Handle, ThresholdClass, JNIEnv.GetMethodID(ThresholdClass, "onCreateViewHolder", "(Landroid/view/ViewGroup;I)Landroidx/recyclerview/widget/RecyclerView$ViewHolder;"), __args), JniHandleOwnership.TransferLocalRef) as RecyclerView.ViewHolder;
                return __ret;
            }
            finally
            {
            }
        }
    }
}