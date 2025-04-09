using Sandbox;

public sealed class TraceBody : Component
{
	[Property, RequireComponent] ModelRenderer ModelRenderer { get; set; }
	[Property, RequireComponent] Collider Collider { get; set; }

	protected override void OnUpdate()
	{
		ModelRenderer.Tint = Color.FromRgba( 0xffffff50 );

		var bbox = Collider.KeyframeBody.GetBounds();
		var boxCenter = bbox.Center;

		DebugOverlay.Sphere( new Sphere( boxCenter, 1f ), Color.Green );
		DebugOverlay.Sphere( new Sphere( WorldPosition, 2f ), Color.Orange );

		var results = Game.ActiveScene.Trace.Body(
			Collider.KeyframeBody,
			WorldPosition )
			.IgnoreGameObject( Collider.GameObject )
			.UseHitPosition( true )
			.RunAll();

		Log.Info( "count " + results.Count() );

		foreach ( var result in results )
		{
			var sphere = new Sphere();
			sphere.Center = result.HitPosition;
			sphere.Radius = 5f;
			DebugOverlay.Sphere( sphere, Color.Red );

			if ( !result.GameObject.Components.TryGet( out ModelRenderer touchedModelRender )) { return; }

			DebugOverlay.Model( touchedModelRender.Model, Color.Green, 0, touchedModelRender.Transform.World );

		}
	}
}
