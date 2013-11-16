using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq.Expressions;


namespace Lync_Backend.Helpers
{
    public class ObjectActivatorHelper
    {

        public delegate T ObjectActivator<T>(params object[] args);

        public  static ObjectActivator<T> GetActivator<T> (ConstructorInfo ctor)
        {
            Type type = ctor.DeclaringType;
            ParameterInfo[] paramsInfo = ctor.GetParameters();

            //create a single param of type object[]
            ParameterExpression param = Expression.Parameter(typeof(object[]), "args");

            Expression[] argsExp = new Expression[paramsInfo.Length];

            //pick each arg from the params array 
            //and create a typed expression of them
            for (int i = 0; i < paramsInfo.Length; i++)
            {
                Expression index = Expression.Constant(i);
                Type paramType = paramsInfo[i].ParameterType;

                Expression paramAccessorExp =
                    Expression.ArrayIndex(param, index);

                Expression paramCastExp =
                    Expression.Convert(paramAccessorExp, paramType);

                argsExp[i] = paramCastExp;
            }

            //make a NewExpression that calls the
            //ctor with the args we just created
            NewExpression newExp = Expression.New(ctor, argsExp);

            //create a lambda with the New
            //Expression as body and our param object[] as arg
            LambdaExpression lambda =
                Expression.Lambda(typeof(ObjectActivator<T>), newExp, param);

            //compile it
            ObjectActivator<T> compiled = (ObjectActivator<T>)lambda.Compile();
            return compiled;
        }
    }

    public class ActivatorsStorage
    {
        public delegate object ObjectActivator(params object[] args);

        private readonly Dictionary<string, ObjectActivator> activators = new Dictionary<string, ObjectActivator>();

        private ObjectActivator CreateActivator(ConstructorInfo ctor)
        {
            Type type = ctor.DeclaringType;

            ParameterInfo[] paramsInfo = ctor.GetParameters();
            ParameterExpression param = Expression.Parameter(typeof(object[]), "args");

            Expression[] argsExp = new Expression[paramsInfo.Length];

            for (int i = 0; i < paramsInfo.Length; i++)
            {
                Expression index = Expression.Constant(i);
                Type paramType = paramsInfo[i].ParameterType;

                Expression paramAccessorExp = Expression.ArrayIndex(param, index);

                Expression paramCastExp = Expression.Convert(paramAccessorExp, paramType);

                argsExp[i] = paramCastExp;
            }

            NewExpression newExp = Expression.New(ctor, argsExp);

            LambdaExpression lambda = Expression.Lambda(typeof(ObjectActivator), newExp, param);

            return (ObjectActivator)lambda.Compile();
        }

        private ObjectActivator CreateActivator(string className)
        {
            Type type = Type.GetType(className);
            
            if (type == null)
                throw new ArgumentException("Incorrect class name", "className");

            // Get contructor with one parameter
            ConstructorInfo ctor = type.GetConstructors().SingleOrDefault(w => w.GetParameters().Length == 1  && w.GetParameters()[0].ParameterType == typeof(object));
            //MethodInfo method =  type.GetMethod(methodName)(w => w.GetParameters().Length == 1  && w.GetParameters()[0].ParameterType == typeof(object));


            if (ctor == null)
                throw new Exception("There is no any constructor with 1 object parameter.");

            return CreateActivator(ctor);
        }

        public ObjectActivator GetActivator(string className)
        {
            ObjectActivator activator;

            if (activators.TryGetValue(className, out activator))
            {
                return activator;
            }
            activator = CreateActivator(className);
            activators[className] = activator;
            return activator;
        }
    }

    public static class NewTpes<T> where T : new()
    {
        public static readonly Func<T> Instance = Expression.Lambda<Func<T>>( Expression.New(typeof(T)) ).Compile();
    }

   
}
