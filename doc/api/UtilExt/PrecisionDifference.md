# SearchAThing.UtilExt.PrecisionDifference method
## PrecisionDifference(double, double)
Compute precision difference between two numbers
            so that results will be a positive number >= 0
            that measures how much they equals.
            For example a result of 1e-6 states that two given numbers are equals until 6 precision digits.
            111111111111111d.PrecisionDifference(111111011111111d) == 1.000000000139778E-06
            
            The function useful to states if two computation results equals except for some precision digits.
            
            Other examples:
            111111111111111d.PrecisionDifference(111111111111111d) == 0
            111111111111111d.PrecisionDifference(191111111111111d) == 0.8
            111111111111111d.PrecisionDifference(111111111111119d) == 7.9936057773011271E-14
            1.23e-210.PrecisionDifference(1.23001e-210) == 9.9999999998434674E-06
            21.23e-210.PrecisionDifference(1.23001e-210) == 10.892990000000001

### Signature
```csharp
public static double PrecisionDifference(double a, double b)
```
### Returns

### Remarks

