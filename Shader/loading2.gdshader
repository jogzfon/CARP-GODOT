shader_type canvas_item;

uniform vec3 color_start : source_color = vec3(0.2, 0.6, 1.0); // Starting color
uniform vec3 color_end : source_color = vec3(0.8, 0.2, 0.8);   // Ending color
uniform float speed : hint_range(0.1, 5.0, 0.1) = 1.0;         // Rotation speed
uniform float thickness : hint_range(0.0, 1.0, 0.01) = 0.1;    // Thickness of the circular segments
uniform float segments : hint_range(3.0, 12.0, 1.0) = 8.0;     // Number of segments in the loading circle
uniform float glow_intensity : hint_range(0.0, 3.0, 0.1) = 1.0; // Glow effect intensity
uniform float pulse_speed : hint_range(0.0, 5.0, 0.1) = 0.5;   // Speed of the pulse effect

void fragment() {
    vec2 uv = UV - 0.5;
    float r = length(uv);             // Distance from center
    float angle = atan(uv.y, uv.x);    // Angle for circular pattern

    float time_offset = TIME * speed;  // Time-based rotation
    float segment_angle = TAU / segments;  // Calculate the angle for each segment

    // Define circular segments based on the current angle and offset by time
    float spiral = fract((angle + TAU + time_offset) / segment_angle);

    // Control thickness of the circular segments
    float mask = smoothstep(thickness, thickness * 0.5, abs(spiral - 0.5));

    // Calculate pulsing effect for smooth transitions
    float pulse = 0.5 + 0.5 * sin(TIME * pulse_speed);

    // Interpolate between two colors
    vec3 color = mix(color_start, color_end, pulse);

    // Apply glow based on intensity and distance from the center
    float glow = glow_intensity * smoothstep(0.4, 0.0, r);

    // Control the fading effect near the edges for a smooth transition
    float alpha = smoothstep(0.5, 0.0, r);

    // Final output color with added glow
    COLOR = vec4(color + glow, alpha * mask);
}
