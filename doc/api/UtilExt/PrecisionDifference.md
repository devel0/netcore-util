# SearchAThing.UtilExt.PrecisionDifference method
## PrecisionDifference(double, double)
Measure precision distance between two given number.
            for example two big numbers 1234567890123.0 and 1234567890023
            have difference of 100 but a precision difference of about 1e-10.
            This is an instrumentation function that is not to be used outside its scope,
            it will help to understand loss of precision between two numbers represented with different storage.
            For example this could useful to compare if an import-export tool produce results comparable to other previous versions
            because there can be approximations around 1e-12 and 1e-15 due to different format and providers.  
            While in general to compare measurements a tolerance have to be used and EqualsTol method, so that
            1234567890123.0d.EqualsTol(1e-10, 1234567890023) is false because diff is 100.

### Signature
```csharp
public static double PrecisionDifference(double a, double b)
```
### Returns

### Remarks

