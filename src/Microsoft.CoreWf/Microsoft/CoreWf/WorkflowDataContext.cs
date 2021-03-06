// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.CoreWf.Runtime;
using System;
using System.ComponentModel;

namespace Microsoft.CoreWf
{
    [Fx.Tag.XamlVisible(false)]
    public sealed class WorkflowDataContext : /*CustomTypeDescriptor,*/ INotifyPropertyChanged, IDisposable
    {
        private ActivityExecutor _executor;
        private ActivityInstance _activityInstance;
        //IDictionary<Location, PropertyDescriptorImpl> locationMapping;
        private PropertyChangedEventHandler _propertyChangedEventHandler;
        //PropertyDescriptorCollection properties;
        private ActivityContext _cachedResolutionContext;

        internal WorkflowDataContext(ActivityExecutor executor, ActivityInstance activityInstance, bool includeLocalVariables)
        {
            _executor = executor;
            _activityInstance = activityInstance;
            this.IncludesLocalVariables = includeLocalVariables;
            //this.properties = CreateProperties();
        }

        internal bool IncludesLocalVariables
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // We want our own cached ActivityContext rather than using this.executor.GetResolutionContext
        // because there is no synchronization of access to the executor's cached object and access thru
        // this WorkflowDataContext will not be done on the workflow runtime thread.
        //ActivityContext ResolutionContext
        //{
        //    get
        //    {
        //        ThrowIfEnvironmentDisposed();
        //        if (this.cachedResolutionContext == null)
        //        {
        //            this.cachedResolutionContext = new ActivityContext(this.activityInstance, this.executor);
        //            this.cachedResolutionContext.AllowChainedEnvironmentAccess = true;
        //        }
        //        else
        //        {
        //            this.cachedResolutionContext.Reinitialize(this.activityInstance, this.executor);
        //        }
        //        return this.cachedResolutionContext;
        //    }
        //}

        //PropertyChangedEventHandler PropertyChangedEventHandler
        //{
        //    get
        //    {
        //        if (this.propertyChangedEventHandler == null)
        //        {
        //            this.propertyChangedEventHandler = new PropertyChangedEventHandler(this.OnLocationChanged);
        //        }
        //        return this.propertyChangedEventHandler;
        //    }
        //}

        //PropertyDescriptorCollection CreateProperties()
        //{
        //    // The name in child Activity will shadow the name in parent.
        //    Dictionary<string, object> names = new Dictionary<string, object>();

        //    List<PropertyDescriptorImpl> propertyList = new List<PropertyDescriptorImpl>();

        //    LocationReferenceEnvironment environment = this.activityInstance.Activity.PublicEnvironment;
        //    bool isLocalEnvironment = true;
        //    while (environment != null)
        //    {
        //        foreach (LocationReference locRef in environment.GetLocationReferences())
        //        {
        //            if (this.IncludesLocalVariables || !isLocalEnvironment || !(locRef is Variable))
        //            {
        //                AddProperty(locRef, names, propertyList);
        //            }
        //        }

        //        environment = environment.Parent;
        //        isLocalEnvironment = false;
        //    }

        //    return new PropertyDescriptorCollection(propertyList.ToArray(), true);
        //}

        //void AddProperty(LocationReference reference, Dictionary<string, object> names,
        //    List<PropertyDescriptorImpl> propertyList)
        //{
        //    if (!string.IsNullOrEmpty(reference.Name) &&
        //        !names.ContainsKey(reference.Name))
        //    {
        //        names.Add(reference.Name, reference);
        //        PropertyDescriptorImpl property = new PropertyDescriptorImpl(reference);
        //        propertyList.Add(property);
        //        AddNotifyHandler(property);
        //    }
        //}

        //void AddNotifyHandler(PropertyDescriptorImpl property)
        //{
        //    ActivityContext activityContext = this.ResolutionContext;
        //    try
        //    {
        //        Location location = property.LocationReference.GetLocation(activityContext);
        //        INotifyPropertyChanged notify = location as INotifyPropertyChanged;
        //        if (notify != null)
        //        {
        //            notify.PropertyChanged += PropertyChangedEventHandler;

        //            if (this.locationMapping == null)
        //            {
        //                this.locationMapping = new Dictionary<Location, PropertyDescriptorImpl>();
        //            }
        //            this.locationMapping.Add(location, property);
        //        }
        //    }
        //    finally
        //    {
        //        activityContext.Dispose();
        //    }
        //}

        //void OnLocationChanged(object sender, PropertyChangedEventArgs e)
        //{
        //    PropertyChangedEventHandler handler = this.PropertyChanged;
        //    if (handler != null)
        //    {
        //        Location location = (Location)sender;

        //        Fx.Assert(this.locationMapping != null, "Location mapping must not be null.");
        //        PropertyDescriptorImpl property;
        //        if (this.locationMapping.TryGetValue(location, out property))
        //        {
        //            if (e.PropertyName == "Value")
        //            {
        //                handler(this, new PropertyChangedEventArgs(property.Name));
        //            }
        //            else
        //            {
        //                handler(this, new PropertyChangedEventArgs(property.Name + "." + e.PropertyName));
        //            }
        //        }
        //    }
        //}

        public void Dispose()
        {
            //if (this.locationMapping != null)
            //{
            //    foreach (KeyValuePair<Location, PropertyDescriptorImpl> pair in this.locationMapping)
            //    {
            //        INotifyPropertyChanged notify = pair.Key as INotifyPropertyChanged;
            //        if (notify != null)
            //        {
            //            notify.PropertyChanged -= PropertyChangedEventHandler;
            //        }
            //    }
            //}
        }

        // We need a separate method here from Dispose(), because Dispose currently
        // doesn't make the WDC uncallable, it just unhooks it from notifications.
        internal void DisposeEnvironment()
        {
            _activityInstance = null;
        }

        private void ThrowIfEnvironmentDisposed()
        {
            if (_activityInstance == null)
            {
                throw Microsoft.CoreWf.Internals.FxTrace.Exception.AsError(
                    new ObjectDisposedException(this.GetType().FullName, SR.WDCDisposed));
            }
        }

        //public override PropertyDescriptorCollection GetProperties()
        //{
        //    return this.properties;
        //}

        //class PropertyDescriptorImpl : PropertyDescriptor
        //{
        //    LocationReference reference;
        //    // bool isReadOnly;


        //    public PropertyDescriptorImpl(LocationReference reference)
        //        : base(reference.Name, new Attribute[0])
        //    {
        //        this.reference = reference;
        //    }

        //    public override Type ComponentType
        //    {
        //        get { return typeof(WorkflowDataContext); }
        //    }

        //    public override bool IsReadOnly
        //    {
        //        get
        //        {
        //            // return this.isReadOnly;
        //            return false;
        //        }
        //    }

        //    public override Type PropertyType
        //    {
        //        get
        //        {
        //            return this.reference.Type;
        //        }
        //    }

        //    public LocationReference LocationReference
        //    {
        //        get
        //        {
        //            return this.reference;
        //        }
        //    }

        //    public override bool CanResetValue(object component)
        //    {
        //        return false;
        //    }

        //    public override object GetValue(object component)
        //    {
        //        WorkflowDataContext dataContext = (WorkflowDataContext)component;

        //        ActivityContext activityContext = dataContext.ResolutionContext;
        //        try
        //        {
        //            return this.reference.GetLocation(activityContext).Value;
        //        }
        //        finally
        //        {
        //            activityContext.Dispose();
        //        }
        //    }

        //    public override void ResetValue(object component)
        //    {
        //        throw Microsoft.CoreWf.Internals.FxTrace.Exception.AsError(new NotSupportedException(SR.CannotResetPropertyInDataContext));
        //    }

        //    public override void SetValue(object component, object value)
        //    {
        //        if (IsReadOnly)
        //        {
        //            throw Microsoft.CoreWf.Internals.FxTrace.Exception.AsError(new NotSupportedException(SR.PropertyReadOnlyInWorkflowDataContext(this.Name)));
        //        }

        //        WorkflowDataContext dataContext = (WorkflowDataContext)component;

        //        ActivityContext activityContext = dataContext.ResolutionContext;
        //        try
        //        {
        //            Location location = this.reference.GetLocation(activityContext);
        //            location.Value = value;
        //        }
        //        finally
        //        {
        //            activityContext.Dispose();
        //        }
        //    }

        //    public override bool ShouldSerializeValue(object component)
        //    {
        //        return true;
        //    }
        //}
    }
}
