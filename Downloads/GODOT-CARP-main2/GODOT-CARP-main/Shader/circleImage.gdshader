shader_type canvas_item;

uniform float circle_size : hint_range(0.0, 1.0) = 0.5;
uniform float glow_size : hint_range(0.0, 0.2) = 0.05;
uniform vec4 glow_color : source_color = vec4(1.0, 1.0, 1.0, 0.5);
uniform float ratio_width = 1.0;
uniform float ratio_height = 1.0;
uniform float glow_intensity : hint_range(0.0, 2.0) = 1.0;
uniform float pulse_speed : hint_range(0.1, 5.0) = 1.0;

void fragment() {
    float ratio = ratio_width / ratio_height;
    float dist = distance(vec2(0.5, 0.5), vec2(mix(0.5, UV.x, ratio), UV.y));
    vec4 orig_color = texture(TEXTURE, UV);
    
    // Main circle
    float circle_alpha = 1.0 - smoothstep(circle_size - 0.01, circle_size, dist);
    
    // Outer glow
    float glow_factor = smoothstep(circle_size, circle_size + glow_size, dist);
    glow_factor *= 1.0 - smoothstep(circle_size + glow_size, circle_size + glow_size + 0.01, dist);
    glow_factor *= glow_intensity;
    
    // Pulse effect for the glow
    float pulse = (sin(TIME * pulse_speed) * 0.5 + 0.5) * 0.5 + 0.5;
    glow_factor *= pulse;
    
    // Mix original color and glow
    vec3 final_color = mix(orig_color.rgb, glow_color.rgb, glow_factor * glow_color.a);
    float final_alpha = max(circle_alpha * orig_color.a, glow_factor * glow_color.a);
    
    COLOR = vec4(final_color, final_alpha);
}