# Umbraco SEO Property Editor

This sample demonstrates how to create a simple property editor for Umbraco v14-v16 that collects text from specified field names and uses an AI service to generate SEO optimized content.

The property editor reads an API key from `appsettings.json` and posts the selected field values to a custom API controller which communicates with OpenAI (or any compatible service). The response is displayed in the editor for content editors to use.

## Files

- `SeoContentGeneratorDataEditor.cs` – registers the property editor.
- `SeoContentGeneratorApiController.cs` – API controller that calls the AI service.
- `Views/SeoContentGeneratorEditor.html` – simple UI for the editor.
- `wwwroot/js/seoEditor.controller.js` – AngularJS controller logic.

## appsettings.json snippet

```json
{
  "OpenAI": {
    "ApiKey": "YOUR_API_KEY"
  }
}
```

## Usage

1. Copy these files into an Umbraco web project under `~/App_Plugins/SeoPropertyEditor`.
2. Add your API key to `appsettings.json` under the `OpenAI` section.
3. Build and run the project. The editor can now be added to document types.
4. Configure the editor with the field names you want to crawl (e.g., `name,title,body`). When the button is clicked, the AI-generated SEO text will appear in the editor.

This code is meant as a starting point and may require adjustments depending on your project setup.
