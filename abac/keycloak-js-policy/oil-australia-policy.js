var context = $evaluation.getContext();
var identity = context.getIdentity();
var attributes = identity.getAttributes();
var contextAttributes = context.getAttributes();

//DEBUG - BEGIN

var attributeMap = attributes.toMap();
for (var attribute in attributeMap) {
    var values = attributes.getValue(attribute);
    for (var i = 0; i < values.size(); i++) {
        print(attribute + ": " + values.asString(i));
    }
}

print('===============================================');

var contextAttributesMap = contextAttributes.toMap();
for (var attribute in contextAttributesMap) {
    var values = contextAttributes.getValue(attribute);
    for (var i = 0; i < values.size(); i++) {
        print(attribute + ": " + values.asString(i));
    }
}

//DEBUG - END


var department = attributes.getValue('department') ? attributes.getValue('department').asString(0) : null;
var location = attributes.getValue('location') ? attributes.getValue('location').asString(0) : null;

if (department && location) {
    // Restrict access to users in the "Oil" department and location "Australia"
    if (department === 'Oil' && location === 'Australia') {
        $evaluation.grant();
    }
}