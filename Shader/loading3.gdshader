shader_type canvas_item;

uniform vec3 color_start : source_color = vec3(0.2, 0.6, 1.0); // Starting color
uniform vec3 color_end : source_color = vec3(0.8, 0.2, 0.8);   // Ending color
uniform float rotation_speed : hint_range(0.1, 5.0) = 1.0;      // Rotation speed
uniform float dot_size : hint_range(0.01, 0.1) = 0.05;         // Size of the dots
uniform int num_dots : hint_range(5, 12) = 9;                  // Number of dots
uniform float radius : hint_range(0.1, 0.4) = 0.25;             // Radius of the dot arrangement
uniform float fade_speed : hint_range(0.5, 5.0) = -3;         // Fade speed
uniform float glow_intensity : hint_range(0.0, 3.0) = 1.0;     // Glow effect intensity

float circle(vec2 uv, vec2 center, float size) {
    float dist = length(uv - center);
    return smoothstep(size, size * 0.9, dist);
}

void fragment() {
    vec2 uv = UV - 0.5;              // Center the UV coordinates
    float angle_step = TAU / float(num_dots); // Angle step for dot positioning
    float base_angle = TIME * rotation_speed;  // Base angle for rotation

    vec4 final_color = vec4(0.0); // Initialize final color

    for (int i = 0; i < num_dots; i++) {
        float angle = base_angle + float(i) * angle_step; // Calculate angle for each dot
        vec2 dot_pos = vec2(cos(angle), sin(angle)) * radius; // Position of the dot
        
        // Pulsing effect for dots
        float dot_alpha = 0.3 + 0.7 * sin(TIME * fade_speed + float(i) * angle_step);
        // Color interpolation between start and end colors
        float pulse = 0.5 + 0.5 * sin(TIME * fade_speed);
        vec3 color = mix(color_start, color_end, pulse);
        
        // Create the dot color with applied alpha
        vec4 current_dot_color = vec4(color, 1.0);
        current_dot_color.a *= dot_alpha; // Adjust alpha for fading effect
        
        final_color += current_dot_color * circle(uv, dot_pos, dot_size); // Accumulate colors
    }
    
    // Set the final color output
    COLOR = final_color;
}
