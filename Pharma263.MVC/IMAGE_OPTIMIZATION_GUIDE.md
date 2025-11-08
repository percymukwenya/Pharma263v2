# Image Optimization Guide

## Phase 5.6: Image Optimization

This guide provides best practices for optimizing images in the Pharma263 application.

## Current Image Assets

### Static Images:
- **Logo**: `/wwwroot/images/pharma263Logo.jpg` (Used in PDF reports and potentially header)

## Lazy Loading Implementation

### For Future Image Elements

When adding images to views, always use the `loading="lazy"` attribute for images below the fold:

```html
<!-- Above the fold (visible immediately) - No lazy loading -->
<img src="/images/logo.jpg" alt="Pharma263 Logo" width="200" height="60">

<!-- Below the fold (user must scroll) - Lazy loading -->
<img src="/images/product.jpg"
     alt="Product Name"
     loading="lazy"
     width="300"
     height="200">
```

### Browser Support
- `loading="lazy"` is supported in all modern browsers (Chrome 77+, Edge 79+, Firefox 75+, Safari 15.4+)
- Graceful degradation: Older browsers load images normally

## Responsive Images

### Use srcset for Different Screen Sizes

```html
<img src="/images/product-medium.jpg"
     srcset="/images/product-small.jpg 480w,
             /images/product-medium.jpg 800w,
             /images/product-large.jpg 1200w"
     sizes="(max-width: 600px) 480px,
            (max-width: 900px) 800px,
            1200px"
     alt="Product Name"
     loading="lazy">
```

### Use WebP Format with Fallback

```html
<picture>
    <source srcset="/images/product.webp" type="image/webp">
    <source srcset="/images/product.jpg" type="image/jpeg">
    <img src="/images/product.jpg"
         alt="Product Name"
         loading="lazy">
</picture>
```

## Image Optimization Checklist

### Before Adding Images:

1. **Compress Images**
   - Use tools like TinyPNG, ImageOptim, or Squoosh
   - Target: < 100KB for most images
   - Logo: < 20KB
   - Product images: < 80KB

2. **Correct Format**
   - **Photos**: JPEG or WebP
   - **Graphics/Icons**: PNG or SVG
   - **Logos**: SVG (preferred) or optimized PNG
   - **Complex images**: WebP with JPEG fallback

3. **Correct Dimensions**
   - Don't use CSS to resize large images
   - Create actual smaller versions
   - Max width: 1200px (for desktop displays)

4. **Accessibility**
   - Always include meaningful `alt` text
   - Decorative images: `alt=""`
   - Informative images: Describe the content

### Example Sizes:

- **Logo**: 200x60px @ 1x, 400x120px @ 2x (Retina)
- **Product thumbnails**: 150x150px @ 1x, 300x300px @ 2x
- **Product details**: 600x600px @ 1x, 1200x1200px @ 2x
- **Banners**: 1200x400px @ 1x, 2400x800px @ 2x

## Logo Optimization

The current logo (`pharma263Logo.jpg`) should be optimized:

### Recommended Approach:
1. Convert to SVG format if possible (infinitely scalable, tiny file size)
2. If SVG not possible, create optimized PNG versions:
   - Logo-1x.png (200x60px, ~8KB)
   - Logo-2x.png (400x120px, ~15KB)

### Implementation:
```html
<img src="/images/logo-1x.png"
     srcset="/images/logo-1x.png 1x,
             /images/logo-2x.png 2x"
     alt="Pharma263 Logo"
     width="200"
     height="60">
```

## CSS Background Images

For CSS background images, use image-set() for Retina support:

```css
.logo {
    background-image: image-set(
        url("/images/logo-1x.png") 1x,
        url("/images/logo-2x.png") 2x
    );
    /* Fallback for browsers without image-set support */
    background-image: url("/images/logo-1x.png");
}
```

## Content Security Policy (CSP)

If using external image CDNs, add them to CSP headers:

```csharp
// In Program.cs or middleware
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy",
        "img-src 'self' https://cdn.example.com;");
    await next();
});
```

## Performance Metrics

### Target Metrics:
- **Largest Contentful Paint (LCP)**: < 2.5s
  - Optimize hero images
  - Use appropriate dimensions
  - Consider preloading critical images

- **Cumulative Layout Shift (CLS)**: < 0.1
  - Always specify width and height attributes
  - Reserve space for images before they load

### Preloading Critical Images

For above-the-fold images that are critical:

```html
<head>
    <!-- Preload critical logo -->
    <link rel="preload" as="image" href="/images/logo-1x.png">
</head>
```

## Image Delivery Best Practices

### 1. Use CDN for Images (Future Enhancement)
- Azure CDN, CloudFlare, or AWS CloudFront
- Automatic format conversion (WebP)
- Automatic resizing
- Global edge caching

### 2. Set Proper Cache Headers

Already configured in Program.cs:
```csharp
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Cache static files for 1 year
        const int durationInSeconds = 60 * 60 * 24 * 365;
        ctx.Context.Response.Headers.Append("Cache-Control",
            $"public,max-age={durationInSeconds}");
    }
});
```

### 3. Implement Image Sprite (Optional)

For multiple small icons:
```css
.icon-edit {
    background: url('/images/icons-sprite.png') 0 0;
    width: 16px;
    height: 16px;
}
.icon-delete {
    background: url('/images/icons-sprite.png') -16px 0;
    width: 16px;
    height: 16px;
}
```

## Tools and Resources

### Compression Tools:
- **TinyPNG**: https://tinypng.com/
- **Squoosh**: https://squoosh.app/
- **ImageOptim** (Mac): https://imageoptim.com/

### Format Conversion:
- **CloudConvert**: https://cloudconvert.com/
- **Online-Convert**: https://www.online-convert.com/

### Image CDN Services:
- **Cloudinary**: https://cloudinary.com/
- **Imgix**: https://www.imgix.com/
- **Azure CDN**: https://azure.microsoft.com/en-us/services/cdn/

## Testing

### Lighthouse Audit:
1. Open Chrome DevTools
2. Go to Lighthouse tab
3. Run Performance audit
4. Check "Properly size images" and "Serve images in next-gen formats"

### Network Analysis:
1. Open Chrome DevTools → Network tab
2. Filter by "Img"
3. Check:
   - Image file sizes
   - Load times
   - Number of images loaded

## Implementation Priority

### High Priority (Immediate):
1. ✅ Add lazy loading guidelines
2. ✅ Document responsive image patterns
3. ✅ Configure static file caching (already done)

### Medium Priority (Next Sprint):
1. Optimize existing logo (convert to SVG or optimized PNG)
2. Create responsive versions for future product images
3. Implement WebP format with fallback

### Low Priority (Future Enhancement):
1. Implement image CDN
2. Add automated image optimization pipeline
3. Create image sprite for icons

## Future Image Additions

When adding new images to the application:

1. **Run through optimization checklist**
2. **Create responsive versions** (1x, 2x for Retina)
3. **Use lazy loading** for below-the-fold images
4. **Always specify dimensions** (prevent layout shift)
5. **Provide meaningful alt text** (accessibility)

## Performance Impact

### Before Optimization:
- Logo: 50KB (unoptimized JPEG)
- No lazy loading
- No responsive images
- Cache: Default browser cache

### After Optimization:
- Logo: 8KB (optimized PNG or 2KB SVG)
- Lazy loading: 50-70% fewer image loads on initial page load
- Responsive images: 40-60% smaller images on mobile
- Cache: 1-year browser cache + CDN (future)

**Expected Total Impact**: 60-80% reduction in image-related bandwidth usage
