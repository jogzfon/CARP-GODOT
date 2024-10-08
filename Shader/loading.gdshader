shader_type canvas_item;

uniform int spiral_type : hint_range(1, 5) = 1;
uniform vec3 primary_color : source_color = vec3(0.2, 0.6, 1.0);
uniform vec3 secondary_color : source_color = vec3(0.8, 0.2, 0.8);
uniform float rays : hint_range(0.0, 20.0, 1.0) = 6.0;
uniform float speed : hint_range(0.0, 20.0, 0.01) = 1.0;
uniform float fade : hint_range(0.0, 3.0, 0.01) = 0.5;
uniform float thickness : hint_range(0.0, 1.0, 0.01) = 0.5;
uniform bool clockwise = true;
uniform float tiers : hint_range(0.0, 20.0, 1.0) = 4.0;
uniform float stretch : hint_range(0.0, 10.0, 0.1) = 6.28;
uniform float glow_intensity : hint_range(0.0, 5.0, 0.1) = 1.0;
uniform float pulse_speed : hint_range(0.0, 5.0, 0.01) = 1.0;

void fragment() {
    vec2 uv = UV - 0.5;
    float r = length(uv);
    float angle = atan(uv.y, uv.x);

    // Calculate the pulsing effect
    float pulse = 0.5 + 0.5 * sin(TIME * pulse_speed);

    float spiral = 0.0;
    float direction = clockwise ? 1.0 : -1.0;
    float t = direction * TIME * speed;

    // Spiral calculations
    if (spiral_type == 1 || spiral_type == 2) {
        spiral = fract((2.0 * r - (angle + PI) / TAU) * rays + t);
        spiral = spiral_type == 1 ? 1.0 - smoothstep(-0.1, 0.1, spiral - thickness) :
                                    1.0 - smoothstep(0.0, thickness, abs(0.5 - spiral));
    } else if (spiral_type == 3 || spiral_type == 4) {
        spiral = fract(tiers * (2.0 * r)) / tiers - mod(tiers * (angle + PI) - t, TAU) / (stretch * tiers);
        spiral = spiral_type == 3 ? 1.0 - smoothstep(0.0, thickness, spiral) :
                                    1.0 - smoothstep(0.0, thickness, abs(spiral));
    } else if (spiral_type == 5) {
        spiral = fract((2.0 * r - (angle + PI) / TAU) * rays + t);
    }

    // Gradient and glow effect
    vec3 color = mix(primary_color, secondary_color, spiral * pulse);
    float glow = glow_intensity * smoothstep(0.0, thickness, spiral);

    // Alpha with a stronger fade effect
    float alpha = spiral_type == 5 ? 1.0 : spiral;
    float distance_fade = pow(1.0 - r * 2.0, fade * 4.0);

    // Final color output
    COLOR = vec4(color + glow, alpha * distance_fade);
}
