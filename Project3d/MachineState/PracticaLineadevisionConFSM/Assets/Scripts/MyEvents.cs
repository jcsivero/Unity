public class MyEvents
{    
    
    public delegate bool Delegate_();
    public event Delegate_ delegate_;
    
    public void AddListener(Delegate_ d)
    {
        delegate_ += d;
    }
    
    public void RemoveListener(Delegate_ d)
    {
        delegate_ -= d;
    }

    public void RemoveAllListeners()
    {
        delegate_ = null;        
    } 
    public bool Invoke()
    {
        
        if (delegate_ == null)        
            return false;            
        else        
           return delegate_();
                                                     
    }
  
}
public class MyEvents<T>
{    
    
    public delegate void Delegate_(T data, EventDataReturned valueToReturn);
    public event Delegate_ delegate_;
    
    public void AddListener(Delegate_ d)
    {
        delegate_ += d;
    }
    
    public void RemoveListener(Delegate_ d)
    {
        delegate_ -= d;
    }

    public void RemoveAllListeners()
    {
        delegate_ = null;        
    } 
    public  void Invoke(T data, EventDataReturned valueToReturn)
    {
        
        if (delegate_ == null)        
            valueToReturn.succes_ = false;            
        else        
           delegate_(data,valueToReturn);
                                                     
    }
  
}