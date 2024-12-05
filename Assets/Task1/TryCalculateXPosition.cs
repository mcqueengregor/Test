using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TryCalculateXPosition
{
    // Used to calculate where a point 'p' will be on the x-axis when it reaches height 'h' after bouncing within a rectangle of width 'w', if it reaches the height at all:
    bool TryCalculateXPositionAtHeight(
        float h,
        Vector2 p,
        Vector2 v,
        float G,
        float w,
        ref float xPosition)
    {
        // Determine if the ball will actually reach height h, early-out if it won't:
        // (calculate discriminant of quadratic applied to equation of displacement, if < 0 then the height isn't reached)
        float yDisplacement = h - p.y;
        float discriminant = (v.y * v.y) * 0.25f - (4 * G * yDisplacement);

        if (discriminant < 0)   // The projectile's parabolic path has no roots with y = h, therefore the height 'h' isn't reached by the ball
            return false;

        // Calculate the horizontal displacement of the ball after reaching height as if it wasn't within the rectangle:
        // (find time that the ball reaches height 'h' at, substitute into equation of displacement to find x-displacement)
        bool useFirstOrSecondRoot = p.y < h;    // If the ball starts below the specified height then use the first intersection with 'h', else use the second
        float t = useFirstOrSecondRoot ? 
            (-G * 0.5f - Mathf.Sqrt(discriminant)) / (2f * v.y) :
            (-G * 0.5f + Mathf.Sqrt(discriminant)) / (2f * v.y);

        float xDisplacement = v.x * t + 0.5f * G * (t * t);

        // Calculate the remianing amount of x-displacement after final bounce (use different formula depending on sign of starting x-velocity):
        int numBounces = v.x > 0 ?
            Mathf.FloorToInt((p.x + Mathf.Abs(xDisplacement)) / w) :
            Mathf.FloorToInt((w - p.x) + Mathf.Abs(xDisplacement) / w);
        
        // Determine final ball position on x-axis after all bounces (based on number of bounces and sign of 'v.x'):
        float displacementAfterBounces;

        if (numBounces % 2 == 0 && v.x > 0f)        // Ball starts moving in +ve x-direction and bounces an even number of times (final bounce is from the left)
        {
            displacementAfterBounces = p.x + xDisplacement - (numBounces * w);
            xPosition = displacementAfterBounces;
        }
        else if (numBounces % 2 != 0 && v.x > 0f)   // Ball starts moving in +ve x-direction and bounces an odd number of times (final bounce is from the right)
        {
            displacementAfterBounces = p.x + xDisplacement - (numBounces * w);
            xPosition = w - displacementAfterBounces;
        }
        else if (numBounces % 2 == 0 && v.x <= 0f)  // Ball starts moving in -ve x-direction and bounces an even number of times (final bounce is from the right)
        {
            displacementAfterBounces = (w - p.x) + xDisplacement - (numBounces * w);
            xPosition = w - displacementAfterBounces;
        }
        else                                        // Ball starts moving in -ve x-direction and bounces an odd number of times (final bounce is from the left)
        {
            displacementAfterBounces = (w - p.x) + xDisplacement - (numBounces * w);
            xPosition = displacementAfterBounces;
        }

        return true;
    }
}
