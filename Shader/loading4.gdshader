shader_type canvas_item;

uniform vec3 color_start : source_color = vec3(0.2, 0.6, 1.0);
uniform vec3 color_end : source_color = vec3(0.8, 0.2, 0.8);
uniform float speed : hint_range(0.1, 5.0) = -1.0;                // Speed of the rotation
uniform float thickness : hint_range(0.0, 0.5) = 0.1;           // Thickness of the circular segments
uniform float segments : hint_range(3.0, 12.0) = 8.0;           // Number of segments in the loading circle
uniform float glow_intensity : hint_range(0.0, 3.0) = 1.0;      // Glow effect intensity
uniform float pulse_speed : hint_range(0.0, 5.0) = 0.5;         // Speed of the pulse effect
uniform float dot_size : hint_range(0.01, 0.1) = 0.05;          // Size of the polka dots
uniform int num_dots : hint_range(5, 12) = 8;                   // Number of dots
uniform float dot_radius : hint_range(0.1, 0.4) = 0.3;          // Distance from center to dots
uniform float fade_speed : hint_range(0.5, 5.0) = -2.0;          // Speed of fading for dots

// Function to create a circular mask
float circle(vec2 uv, vec2 center, float size) {
    float dist = length(uv - center);
    return smoothstep(size, size * 0.9, dist);
}

void fragment() {
    vec2 uv = UV - 0.5;               // Normalize UV coordinates
    float r = length(uv);             // Distance from center
    float angle = atan(uv.y, uv.x);  // Angle for circular pattern
    float time_offset = TIME * speed; // Time-based rotation

    // Reverse the rotation of the outer segments
    float segment_angle = TAU / segments;
    float spiral = fract((angle - TAU + time_offset) / segment_angle); // Reverse rotation
    float mask = smoothstep(thickness, thickness * 0.5, abs(spiral - 0.5));
    float alpha = smoothstep(0.5, 0.0, r);

    // Color pulsing effect
    float pulse = 0.5 + 0.5 * sin(TIME * pulse_speed);
    vec3 color = mix(color_start, color_end, pulse);

    // Glow effect based on distance from the center
    float glow = glow_intensity * smoothstep(0.4, 0.0, r);

    // Inner rotating dots
    float dot_angle_step = TAU / float(num_dots);
    float dot_base_angle = -TIME * speed * 0.5; // Reverse the rotation of dots
    vec4 dot_final_color = vec4(0.0);

    for (int i = 0; i < num_dots; i++) {
        float dot_angle = dot_base_angle + float(i) * dot_angle_step;
        vec2 dot_pos = vec2(cos(dot_angle), sin(dot_angle)) * dot_radius;

        // Fading effect for dots
        float dot_alpha = 0.3 + 0.7 * sin(TIME * fade_speed + float(i) * dot_angle_step);
        vec4 current_dot_color = vec4(color, 1.0);
        current_dot_color.a *= dot_alpha;

        // Add current dot to the final dot color
        dot_final_color += current_dot_color * circle(uv, dot_pos, dot_size);
    }

    // Combine outer segments and inner dots
    vec3 final_color = color + glow;
    final_color = mix(final_color, dot_final_color.rgb, dot_final_color.a);
    float final_alpha = max(alpha * mask, dot_final_color.a);

    // Set the final color and alpha
    COLOR = vec4(final_color, final_alpha);
}
