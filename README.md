# AnimationExpression

Create animation script asset without compile

## Install

Add a following git url on Unity Package Manager:
 - `https://git@github.com/beinteractive/AnimationExpression.git?path=Packages/Animation%20Expression`
 
Add following packages:
 - Uween: https://github.com/beinteractive/Uween/tree/v2
 - AnKuchen: https://github.com/kyubuns/AnKuchen

## How to use

 - `Assets` -> `Create` -> `Animation Expression` to create an asset
 - To edit the asset:
   - Add target GameObject (A) which has AnKuchen's `UICache` to a scene
   - Add another GameObject and add `AnimationExpressionPreview` component (B)
   - Assign (A) to (B)'s `Live Target`
   - Assign the asset to (B)'s `Animation Expression`
   - Now you can edit and preview animations on editor
 
## Script Example
 
```
Active(CancelButton, false);

Y(OkButton, Duration).FromRelative(-100f).EaseOutQuart().Animate(Animate);
Scale(OkButton, Duration).From(0f).Animate(Animate).EaseOutBack().Then({
  Active(CancelButton, true);
  if (Flag) {
    Scale(CancelButton, Duration).From(0f).EaseOutBack().Delay(0.15f).Animate(Animate).Then(
        Callback();
    );
  }
});
```
