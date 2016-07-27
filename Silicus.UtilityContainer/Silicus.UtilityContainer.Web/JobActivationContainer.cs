using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using Hangfire;

public class JobActivationContainer : JobActivator
{

    private Dictionary<Type, Delegate> ParameterMap { get; set; }

    private bool CompareParameterToMap(ParameterInfo p)
    {
        dynamic result = ParameterMap.ContainsKey(p.ParameterType);
        return result;
    }

    public override object ActivateJob(Type jobType)
    {
        System.Reflection.ConstructorInfo candidateCtor = null;
        //Loop through ctor's and find the most specific ctor where map has all types.

        var conInfo = jobType.GetConstructors();
        foreach(var i in conInfo)
        {
            var paramInfo = i.GetParameters();
            List<ParameterInfo> lst = new List<ParameterInfo>(paramInfo);
            if (lst.TrueForAll(CompareParameterToMap))
            {
                if (candidateCtor == null)
                    candidateCtor = i;
                if (!object.ReferenceEquals(i, candidateCtor) && lst.Count > candidateCtor.GetParameters().)
                    candidateCtor = i;
            }
        }

        //jobType.GetConstructors.ToList.ForEach(i =>
        //{
        //    if (i.GetParameters.ToList.TrueForAll(CompareParameterToMap))
        //    {
        //        if (candidateCtor == null)
        //            candidateCtor = i;
        //        if (!object.ReferenceEquals(i, candidateCtor) && i.GetParameters.Count > candidateCtor.GetParameters.Count)
        //            candidateCtor = i;
        //    }
        //});

        if (candidateCtor == null)
        {
            //If the ctor is null, use default activator.
            return base.ActivateJob(jobType);
        }
        else
        {
            //Create a list of the parameters in order and activate
            List<object> ctorParameters = new List<object>();
            candidateCtor.GetParameters.ToList.ForEach(i => ctorParameters.Add(ParameterMap(i.ParameterType).DynamicInvoke()));
            return Activator.CreateInstance(jobType, ctorParameters.ToArray);
        }
    }

    public void RegisterDependency<T>(Func<T> factory)
    {
        if (!ParameterMap.ContainsKey(typeof(T)))
            ParameterMap.Add(typeof(T), factory);
    }

    public JobActivationContainer()
    {
        ParameterMap = new Dictionary<Type, Delegate>();
    }
}

//=======================================================
//Service provided by Telerik (www.telerik.com)
//Conversion powered by NRefactory.
//Twitter: @telerik
//Facebook: facebook.com/telerik
//=======================================================
