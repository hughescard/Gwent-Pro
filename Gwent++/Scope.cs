using System.Reflection;
using interpreter;

public class Scope 
{
    //Variables: key:Variuable name Value: ?? 
    public Dictionary<string,string> Variables;
    public Dictionary<string,TypeInfo> typeInfo;
    public Scope? Parent { get;}
    public Scope(Scope? parent = null)
    {   
        Variables = new Dictionary<string, string>();
        typeInfo = new Dictionary<string, TypeInfo>();
        this.Parent = parent;
    }
    public void Define(string name,string type)
    {
        if(ContainsVariable(name) && Variables[name] != type)
        {
            throw new Exception($"La variable {name} no coincide con el tipo previamente asignado");
        }
        Variables[name] = type;
    }
    public string Resolve(string name)
    {
        if(Variables.ContainsKey(name))return Variables[name];
        if(Parent!=null)return Parent.Resolve(name);
        else 
        throw new Exception("La variable no ha sido definida");
    } 
    public bool ContainsVariable(string name)
    {
        if(Variables.ContainsKey(name))return true;
        else if (Parent!=null)return Parent.ContainsVariable(name);
        else 
            return false;
    }
    public bool ContainsTypeInfo(string name)
        {
            if(typeInfo.ContainsKey(name))
                return true;
            if(Parent == null)
                return false;
            
            return Parent.ContainsTypeInfo(name);     
        }
    public TypeInfo? ResolveTypeInfo(string name)
        {
            if (typeInfo.TryGetValue(name, out var type))
            {
                return type;
            }
            return Parent?.ResolveTypeInfo(name);
        }
   
}