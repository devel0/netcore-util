# SearchAThing.UtilExt.PercentDifference method
## PercentDifference(double, double)
Measure percent difference between given two numbers;
            can return double.NaN if one of two numbers are 0;
            returns 0 if two given numbes are either 0.
            
            Given f = PercentDifference(x, y)
            m = Min(x, y)
            M = Max(x, y)        
            a = Min(Abs(x), Abs(y))
            
            returned value f satisfy follow condition
            
            M(m, a, f) = m + a * f

### Signature
```csharp
public static double PercentDifference(double x, double y)
```
### Returns

### Remarks

