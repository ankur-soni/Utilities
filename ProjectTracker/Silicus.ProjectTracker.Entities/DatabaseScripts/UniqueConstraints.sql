USE @DatabaseName


IF OBJECTPROPERTY(OBJECT_ID('ProjectUniqueName'), 'IsConstraint') = 1
ALTER TABLE Project DROP CONSTRAINT  ProjectUniqueName

ALTER TABLE Project
ADD CONSTRAINT ProjectUniqueName UNIQUE (ProjectName)
