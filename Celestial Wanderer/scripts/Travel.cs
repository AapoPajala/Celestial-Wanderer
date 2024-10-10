using Godot;
using System;

public class Travel : Node2D
{
	Camera2D camera;
	Galaxy galaxy;
	public override void _Ready()
	{
		camera = (Camera2D) GetNode("PlayerCamera");
		ui = camera.GetChild(0) as UI;
		prev = new Vector2();
		to = new Vector2();
		view = NavView.INTERSTELLAR;
		player = (Player) GetNode("Player");
		galaxy = (Galaxy) GetNode("Galaxy");
		Random r = new Random();
		setCurrentStar((Star) galaxy.GetChild(r.Next(galaxy.GetChildCount())));
		player.Position = currentStar.Position;
		camera.Position = player.Position;
	}

	Player player;
	public static NavView view;

	public enum NavView {
		INTERSTELLAR, PLANETARY, ORBITAL, COMBAT, TRADE
	}

	float mtime, dist;
	Vector2 prev, to;
	public override void _Process(float delta)
	{

		float zoomrate = 10f;
		if(view != NavView.INTERSTELLAR) zoomrate = 20f;

        scroll *= 0.95f;
        if (camera.Zoom.x < 16f)
            camera.Zoom += new Vector2(scroll / zoomrate, scroll / zoomrate);
        else {
            scroll *= 0.1f;
            camera.Zoom -= new Vector2(scroll / zoomrate, scroll / zoomrate);
        }

        if (view == NavView.INTERSTELLAR) {
			

			if(camera.Zoom.x < 1f) {
				changeView(NavView.PLANETARY);
			}

			Vector2 mouse = camera.GetGlobalMousePosition();


			if(mb == 1) mtime += delta;
			else mtime = 0;


			dist *= 0.9f;

			if(mtime > 0.1f) {
				dist = prev.DistanceTo(mouse);
			} else {
				prev = mouse;
			}
			if(dist > 0f && mb == 1) to = mouse;

			camera.Position = camera.Position.MoveToward(to, -dist * delta);

			travelToStar();

			

		}

		if (view == NavView.PLANETARY) {

			

			if (camera.Zoom.x < 1f) {
				if(currentSatellite != null) changeView(NavView.ORBITAL);
				else camera.Zoom -= new Vector2(scroll / zoomrate, scroll / zoomrate);
			}

            if (camera.Zoom.x > 5) {
				changeView(NavView.INTERSTELLAR);
            }

			travelToSatellite();
			if(!following && currentSatellite != null) {
				orbitAround(currentSatellite.GlobalPosition);
				camera.GlobalPosition = currentSatellite.GlobalPosition;
			} else orbitAround(currentStar.GlobalPosition);

		}

		if(view == NavView.ORBITAL) {
			if(camera.Zoom.x < 0.4f) {
				camera.Zoom -= new Vector2(scroll / zoomrate, scroll / zoomrate);
			}

			orbitAround(currentSatellite.GlobalPosition);
			travelToSatellite();

			if(camera.Zoom.x > 2f) {
				changeView(NavView.PLANETARY);
			}
		}
	}

	Vector2 travelTo;
	Star currentStar;

	bool traveling, following;
	float travelDist;

	public void orbitAround(Vector2 star) {
		Vector2 pos = new Vector2(player.GlobalPosition.x, player.GlobalPosition.y);

		float angle = 0;
		angle += 0.0017f;

		float s = (float)Math.Sin(angle);
		float c = (float)Math.Cos(angle);

		pos.x -= star.x;
		pos.y -= star.y;

		float xnew = pos.x * c - pos.y * s;
		float ynew = pos.x * s + pos.y * c;

		pos.x = xnew + star.x;
		pos.y = ynew + star.y;

		player.GlobalPosition = pos;
		player.Rotation = player.GlobalPosition.AngleToPoint(star) - 1.37f;
	}

	public void setCurrentStar(Star star) {
        currentStar = star;
        currentStar.SetProcess(true);
    }
	
	Satellite currentSatellite;
	public void setCurrentSatellite(Satellite satellite) {
		currentSatellite = satellite;
	}

	UI ui;

	public void changeView(NavView v) {

		if(v == NavView.INTERSTELLAR) {
			camera.Zoom = new Vector2(1, 1);
            view = NavView.INTERSTELLAR;
            currentStar.children.Visible = false;
            currentStar.Scale = new Vector2(0.25f, 0.25f);
			player.Scale = Vector2.One;
			player.Position = currentStar.Position;
            galaxy.Visible = true;
            currentStar.SetProcess(false);
			currentStar.suspended(true);
			currentSatellite = null;
			Satellite.selectedSatellite = null;
		}

		if(v == NavView.PLANETARY) {
			if(!currentStar.loaded) currentStar.load();
			if(view == NavView.ORBITAL && currentSatellite != null) {
				RemoveChild(currentSatellite);
				currentStar.children.AddChild(currentSatellite);
				Planet p = currentSatellite as Planet;
				if(p != null) p.showMoons(false);
			}
			if(view == NavView.INTERSTELLAR) {
				galaxy.RemoveChild(currentStar);
				AddChild(currentStar);
			}

			view = NavView.PLANETARY;
			camera.Zoom = new Vector2(3, 3);
			galaxy.Visible = false;
			currentStar.suspended(false);
			currentStar.Visible = true;
			currentStar.children.Visible = true;
			currentStar.Update();
			player.Scale = new Vector2(0.5f, 0.5f);
			currentStar.children.Visible = true;
			currentStar.Scale = new Vector2(1, 1);
			if(currentSatellite == null)
				player.Position = currentStar.Position + currentStar.sprite.Texture.GetSize() * 2f;

			if(currentStar.civilization != null) ui.showCiv(currentStar.civilization, true);
		}

		if(v == NavView.ORBITAL) {
			player.Scale = new Vector2(0.1f, 0.1f);
			if(!currentSatellite.loaded) currentSatellite.load();
			currentStar.children.RemoveChild(currentSatellite);
			AddChild(currentSatellite);
			camera.Zoom = new Vector2(0.5f, 0.5f);
			view = NavView.ORBITAL;
			currentStar.Visible = false;
			player.GlobalPosition = currentSatellite.GlobalPosition + new Vector2(currentSatellite.radius * 4f, 0);
			
			Planet p = currentSatellite as Planet;
			if(p != null) p.showMoons(true);
		}

		if(v == NavView.TRADE) {

		}

		if(v == NavView.COMBAT) {

		}
	}

	public void travelToStar() {
		if(Star.selectedStar != null) {
			if(mb == 1 && mtime < 0.1f && travelTo != Star.selectedStar.Position) {
				travelTo = Star.selectedStar.Position;
				traveling = true;
				float ang = player.Position.AngleToPoint(travelTo);
				player.Rotation = ang;
				if(currentStar != null)
					currentStar.suspended(true);

				if(GetChildren().Contains(currentStar)) {
					RemoveChild(currentStar);
					galaxy.AddChild(currentStar);
				}

				setCurrentStar(Star.selectedStar);
				
				following = true;
				travelDist = camera.Position.DistanceTo(travelTo);
			}
		}



		if(traveling) {
			player.Position = player.Position.MoveToward(travelTo, 25f);

			if(player.Position.DistanceTo(travelTo) < 16f)
				traveling = false;

		}

		if(following) {
			float dist = camera.Position.DistanceTo(travelTo);
			float spd = Math.Abs(1 + (1f - (dist / travelDist)) * 100f);

			camera.Position = camera.Position.MoveToward(travelTo, spd);
			if(dist < 4f) following = false;
		}
	}

	public void travelToSatellite() {
		if(Satellite.selectedSatellite != null && Satellite.selectedSatellite != currentSatellite) {
			if(mb == 1 && mtime < 0.1f && travelTo != Satellite.selectedSatellite.Position) {
				travelTo = Satellite.selectedSatellite.GlobalPosition;
				traveling = true;
				float ang = player.GlobalPosition.AngleTo(travelTo);
				player.GlobalRotation = ang;
				setCurrentSatellite(Satellite.selectedSatellite);
				following = true;
				travelDist = camera.GlobalPosition.DistanceTo(travelTo);
			}
		}



		if(traveling) {
			player.GlobalPosition = player.GlobalPosition.MoveToward(travelTo, 10f);

			if(player.Position.DistanceTo(travelTo) < 16f)
				traveling = false;

		}



		if(following) {
			float dist = camera.GlobalPosition.DistanceTo(travelTo);
			float spd = Math.Abs(1 + (1f - (dist / travelDist)) * 10f);

			camera.GlobalPosition = camera.GlobalPosition.MoveToward(travelTo, spd);
			if(dist < 4f) following = false;
		}
	}

	public static float scroll;
	public static int mb;

	public override void _Input(InputEvent @event) {
		if(@event is InputEventMouseButton) {
			InputEventMouseButton e = (InputEventMouseButton)@event;
			if(e.IsPressed()) mb = e.ButtonIndex;
			else mb = -1;
		}
	}

	public override void _UnhandledInput(InputEvent @event) {
		if(@event is InputEventMouseButton) {
			InputEventMouseButton emb = (InputEventMouseButton)@event;
			if(emb.IsPressed()) {
				if(emb.ButtonIndex == (int)ButtonList.WheelUp) {
					scroll = -1f;
				}
				if(emb.ButtonIndex == (int)ButtonList.WheelDown) {
					scroll = 1f;
				}
			   
			}
		}
	}
}
