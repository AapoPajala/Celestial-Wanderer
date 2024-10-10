using Godot;
using System;
using System.Collections.Generic;

public class Galaxy : Node2D
{


    public static int minDist = 256;
    Dictionary<Vector2, Star> stars;

    public override void _Ready() {

        stars = new Dictionary<Vector2, Star>();

        int w = 128;
        int h = 128;
        int x = 0, y = 0;
        int dx = 0;
        int dy = -1;
        Random r = new Random();
        for(int i = 0; i < Math.Pow(Math.Max(w, h), 2); i++) {
            
            if((-w / 2 < x && x <= w / 2) && (-h / 2 < y && y <= h / 2)) {
                if(r.NextDouble() < 0.25f && x*x + y*y < w*w / 4) {
                    float gx = minDist * (1f - ((float)x / w)) * Math.Sign(x);
                    float gy = minDist * (1f - ((float)y / h)) * Math.Sign(y);
                    float dist = (float)Math.Sqrt(x * x + y * y) / (w/2);
                    float sd = dist * dist;
                    float rot = (float)(Math.PI * Math.Pow(dist, 3));
                    Vector2 pos = new Vector2(x * minDist * sd + r.Next(16), y * minDist * sd + r.Next(16));
                    
                    Star star = new Star();
                    star.Position = pos;
                    //stars.Add(pos, star);
                    AddChild(star);
                }
            }

            if(x == y || (x < 0 && x == -y) || (x > 0 && x == 1 - y)) {
                int t = dx;
                dx = -dy;
                dy = t;
            }
            
            x += dx;
            y += dy;
        }

    }
}
